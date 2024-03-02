using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Abstractions.Handlers.Results;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Handlers
{
    public interface IMessageHandlerManager
    {
        Task<IMessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData);
    }
}