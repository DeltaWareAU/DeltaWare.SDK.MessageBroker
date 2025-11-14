using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public abstract class MessageHandler<TMessage> : IMessageHandler where TMessage : class
    {
        protected MessageHandlerResult? Result { get; set; } = null;

        public async ValueTask<MessageHandlerResult> HandleAsync(object message, CancellationToken cancellationToken)
        {
            TMessage messageToProcess;

            try
            {
                messageToProcess = (TMessage)message;
            }
            catch (Exception ex)
            {
                return MessageHandlerResult.Failure(ex, $"Failed to cast incoming message to ({GetType().Name}).");
            }

            try
            {
                await ProcessAsync(messageToProcess, cancellationToken);
            }
            catch (Exception e)
            {
                return MessageHandlerResult.Failure(e);
            }

            return Result ?? MessageHandlerResult.Success();
        }

        protected abstract ValueTask ProcessAsync(TMessage message, CancellationToken cancellationToken);
    }
}
