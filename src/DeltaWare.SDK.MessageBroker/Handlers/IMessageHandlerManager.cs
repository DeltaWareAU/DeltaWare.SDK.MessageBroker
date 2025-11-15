using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Handlers.Results;

namespace DeltaWare.SDK.MessageBroker.Handlers
{
    public interface IMessageHandlerManager
    {
        Task<MessageHandlerResults> HandleMessageAsync(MessageHandlerBinding handlerBinding, string messageData, CancellationToken cancellationToken);
    }
}