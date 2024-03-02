using DeltaWare.SDK.MessageBroker.Abstractions.Handlers.Results;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Handlers
{
    public interface IMessageHandler
    {
        ValueTask<IMessageHandlerResult> HandleAsync(object message);
    }
}
