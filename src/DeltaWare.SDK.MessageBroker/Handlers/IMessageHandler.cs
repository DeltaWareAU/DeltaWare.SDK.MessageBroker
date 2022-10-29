using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public interface IMessageHandler
    {
        ValueTask<IMessageHandlerResult> HandleAsync(object message);
    }
}
