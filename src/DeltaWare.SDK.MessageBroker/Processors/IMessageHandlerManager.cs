using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Processors.Results;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Processors
{
    public interface IMessageHandlerManager
    {
        Task<IMessageHandlerResults> HandleMessageAsync(IMessageHandlerBinding handlerBinding, string messageData);
    }
}