using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using DeltaWare.SDK.MessageBroker.Core.Messages.Interception;
using DeltaWare.SDK.MessageBroker.Core.Messages.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    internal class MessageHandlerManager : IMessageHandlerManager
    {
        private readonly ILogger? _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IMessageSerializer _messageSerializer;

        private readonly MessageInterceptor? _messageInterceptor;

        public MessageHandlerManager(IServiceScopeFactory serviceScopeFactory, IMessageSerializer messageSerializer, MessageInterceptor? messageInterceptor = null, ILogger<MessageHandlerManager>? logger = null)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _messageSerializer = messageSerializer;
            _messageInterceptor = messageInterceptor;

            _logger = logger;
        }

        public async Task<IMessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData)
        {
            object? message;

            try
            {
                message = _messageSerializer.Deserialize(messageData, handlerBinding.MessageType);

                if (message == null)
                {
                    return MessageHandlerResults.Failure("An exception was encountered whilst deserializing the message");
                }
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "An exception was encountered whilst deserializing the message to {messageType}", handlerBinding.MessageType.Name);

                return MessageHandlerResults.Failure(e, "An exception was encountered whilst deserializing the message");
            }

            _messageInterceptor?.OnMessageReceived(message, handlerBinding.MessageType);
            _messageInterceptor?.OnMessageReceivedAsync(message, handlerBinding.MessageType);

            IMessageHandlerResult[] results = new IMessageHandlerResult[handlerBinding.HandlerTypes.Count];

            for (int i = 0; i < handlerBinding.HandlerTypes.Count; i++)
            {
                using IServiceScope scope = _serviceScopeFactory.CreateScope();

                IMessageHandler messageHandler;

                try
                {
                    messageHandler = (IMessageHandler)scope.ServiceProvider.CreateInstance(handlerBinding.HandlerTypes[i]);
                }
                catch (Exception e)
                {
                    _logger?.LogError(e, "An exception was encountered whilst instantiating {handlerName}", handlerBinding.HandlerTypes[i].Name);

                    results[i] = MessageHandlerResult.Failure(e, $"An exception was encountered whilst instantiating {handlerBinding.HandlerTypes[i].Name}");

                    continue;
                }

                _messageInterceptor?.OnMessageExecuting(message, handlerBinding.MessageType);

                IMessageHandlerResult result = await messageHandler.HandleAsync(message);

                if (result.HasException)
                {
                    _messageInterceptor?.OnException(message, handlerBinding.MessageType, result.Exception!);
                }

                _messageInterceptor?.OnMessageExecuted(message, handlerBinding.MessageType);

                results[i] = result;
            }

            return new MessageHandlerResults(results);
        }
    }
}
