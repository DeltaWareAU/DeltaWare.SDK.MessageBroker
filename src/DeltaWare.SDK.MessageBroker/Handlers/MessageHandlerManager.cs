using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Abstractions.Handlers;
using DeltaWare.SDK.MessageBroker.Abstractions.Handlers.Results;
using DeltaWare.SDK.MessageBroker.Core.Messages.Interception;
using DeltaWare.SDK.MessageBroker.Core.Messages.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;

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

            await (ValueTask)_messageInterceptor?.OnMessageReceivedAsync(message, handlerBinding.MessageType)!;

            IMessageHandlerResult[] results = new IMessageHandlerResult[handlerBinding.HandlerTypes.Count];

            for (int i = 0; i < handlerBinding.HandlerTypes.Count; i++)
            {
                results[i] = await ExecuteMessageHandlerAsync(message, handlerBinding.MessageType, handlerBinding.HandlerTypes[i]);
            }

            return new MessageHandlerResults(results);
        }

        private async Task<IMessageHandlerResult> ExecuteMessageHandlerAsync(object message, Type messageType, Type handlerType)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IMessageHandler messageHandler;

            try
            {
                messageHandler = (IMessageHandler)scope.ServiceProvider.CreateInstance(handlerType);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "An exception was encountered whilst instantiating {handlerName}", handlerType.Name);

                return MessageHandlerResult.Failure(e, $"An exception was encountered whilst instantiating {handlerType.Name}");
            }

            _messageInterceptor?.OnMessageExecuting(message, messageType);

            await (ValueTask)_messageInterceptor?.OnMessageExecutingAsync(message, messageType)!;

            IMessageHandlerResult result = await messageHandler.HandleAsync(message);

            if (result.HasException)
            {
                _messageInterceptor?.OnException(message, messageType, result.Exception!);

                await (ValueTask)_messageInterceptor?.OnExceptionAsync(message, messageType, result.Exception!)!;
            }

            _messageInterceptor?.OnMessageExecuted(message, messageType);

            await (ValueTask)_messageInterceptor?.OnMessageExecutedAsync(message, messageType)!;

            return result;
        }
    }
}
