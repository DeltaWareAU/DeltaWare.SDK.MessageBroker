using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public interface IMessageHandlerManager
    {
        Task<IMessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData);
    }
}