using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Core.Broker
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : Message;
    }
}
