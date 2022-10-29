using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers
{
    public interface IMessageHandlerManager
    {
        Task<IMessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData);
    }
}