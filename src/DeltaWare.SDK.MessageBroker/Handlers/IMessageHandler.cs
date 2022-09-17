using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public interface IMessageHandler
    {
        ValueTask<IMessageHandlerResult> HandleAsync(Message message);
    }
}
