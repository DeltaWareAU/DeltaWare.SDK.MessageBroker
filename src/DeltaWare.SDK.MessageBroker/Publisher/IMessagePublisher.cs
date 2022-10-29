using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Publisher
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class;
    }
}
