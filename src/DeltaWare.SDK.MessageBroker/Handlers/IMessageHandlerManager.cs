using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public interface IMessageHandlerManager
    {
        Task<MessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData, CancellationToken cancellationToken);
    }
}