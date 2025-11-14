using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public interface IMessageHandler
    {
        ValueTask<MessageHandlerResult> HandleAsync(object message, CancellationToken cancellationToken);
    }
}
