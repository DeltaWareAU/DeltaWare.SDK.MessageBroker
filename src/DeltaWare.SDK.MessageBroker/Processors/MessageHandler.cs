using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Processors.Results;
using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Processors
{
    public abstract class MessageHandler<TMessage> : IMessageHandler where TMessage : Message
    {
        protected IMessageHandlerResult? Result { get; set; } = null;

        public async Task<IMessageHandlerResult> HandleAsync(Message message)
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

        protected abstract Task ProcessAsync(TMessage message);
    }
}
