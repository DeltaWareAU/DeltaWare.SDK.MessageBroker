using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Handlers.Results;

namespace DeltaWare.SDK.MessageBroker.Handlers
{
    public interface IMessageHandler
    {
        ValueTask<MessageHandlerResult> HandleAsync(object message, CancellationToken cancellationToken);
    }
}
