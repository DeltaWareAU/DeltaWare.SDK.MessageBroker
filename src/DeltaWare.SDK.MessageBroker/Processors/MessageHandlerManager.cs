using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Messages.Serialization;
using DeltaWare.SDK.MessageBroker.Processors.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Processors
{
    internal class MessageHandlerManager : IMessageHandlerManager
    {
        private readonly ILogger? _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IMessageSerializer _messageSerializer;

        public MessageHandlerManager(IServiceScopeFactory serviceScopeFactory, IMessageSerializer messageSerializer, ILogger<MessageHandlerManager>? logger = null)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _messageSerializer = messageSerializer;

            _logger = logger;
        }

        public async Task<IMessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData)
        {
            Message message;

            try
            {
                message = _messageSerializer.Deserialize(messageData, handlerBinding.MessageType);
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "An exception was encountered whilst deserializing the message to {messageType}", handlerBinding.MessageType.Name);

                return MessageHandlerResults.Failure(e, "An exception was encountered whilst deserializing the message");
            }

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

                results[i] = await messageHandler.HandleAsync(message);
            }

            return new MessageHandlerResults(results);
        }
    }
}
