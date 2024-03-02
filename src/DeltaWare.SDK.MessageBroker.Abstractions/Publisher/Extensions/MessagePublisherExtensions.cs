using System;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker.Abstractions.Publisher
{
    public static class MessagePublisherExtensions
    {
        public static Task PublishAsync<TMessage>(this IMessagePublisher messagePublisher, Action<TMessage> messageBuilder, CancellationToken cancellationToken = default)
            where TMessage : class, new()
        {
            TMessage messageToSend = new TMessage();

            messageBuilder.Invoke(messageToSend);

            return messagePublisher.PublishAsync(messageToSend, cancellationToken);
        }
    }
}
