using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Processors.Results;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Processors
{
    public interface IMessageHandler
    {
        ValueTask<IMessageHandlerResult> HandleAsync(Message message);
    }
}
