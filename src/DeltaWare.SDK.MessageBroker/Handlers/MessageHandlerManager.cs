using System;
using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Handlers.Results;
using DeltaWare.SDK.MessageBroker.Messages.Interception;
using DeltaWare.SDK.MessageBroker.Messages.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.MessageBroker.Handlers
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

        public async Task<MessageHandlerResults> HandleMessageAsync(MessageHandlerBinding handlerBinding, string messageData, CancellationToken cancellationToken)
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

            await _messageInterceptor.OnMessageReceivedAsync(message, handlerBinding.MessageType);

            await (ValueTask)_messageInterceptor?.OnMessageReceivedAsync(message, handlerBinding.MessageType)!;

            var results = new MessageHandlerResult[handlerBinding.HandlerTypes.Count];

            for (int i = 0; i < handlerBinding.HandlerTypes.Count; i++)
            {
                results[i] = await ExecuteMessageHandlerAsync(message, handlerBinding.MessageType, handlerBinding.HandlerTypes[i], cancellationToken);
            }

            return new MessageHandlerResults(results);
        }

        private async Task<MessageHandlerResult> ExecuteMessageHandlerAsync(object message, Type messageType, Type handlerType, CancellationToken cancellationToken)
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

            await _messageInterceptor.OnMessageExecutingAsync(message, messageType);

            var result = await messageHandler.HandleAsync(message, cancellationToken);

            if (result.HasException)
            {
                _messageInterceptor.OnExceptionAsync(message, messageType, result.Exception!);

                await (ValueTask)_messageInterceptor?.OnExceptionAsync(message, messageType, result.Exception!)!;
            }

            _messageInterceptor.OnMessageExecutedAsync(message, messageType);

            await (ValueTask)_messageInterceptor?.OnMessageExecutedAsync(message, messageType)!;

            return result;
        }
    }
}
