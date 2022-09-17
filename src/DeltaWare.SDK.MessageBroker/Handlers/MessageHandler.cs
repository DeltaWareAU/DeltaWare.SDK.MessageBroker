using System;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public abstract class MessageHandler<TMessage> : IMessageHandler where TMessage : Message
    {
        protected IMessageHandlerResult? Result { get; set; } = null;

        public async ValueTask<IMessageHandlerResult> HandleAsync(Message message)
        {
            TMessage messageToProcess;

            try
            {
                messageToProcess = (TMessage)message;
            }
            catch (Exception ex)
            {
                return MessageHandlerResult.Failure(ex, $"Failed to cast incoming message for ({GetType().Name}).");
            }

            try
            {
                await ProcessAsync(messageToProcess);
            }
            catch (Exception e)
            {
                return MessageHandlerResult.Failure(e);
            }

            return Result ?? MessageHandlerResult.Success();
        }

        protected abstract ValueTask ProcessAsync(TMessage message);
    }
}
