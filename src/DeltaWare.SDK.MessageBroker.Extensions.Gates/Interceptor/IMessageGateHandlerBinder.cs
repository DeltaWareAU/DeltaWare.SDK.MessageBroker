using DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor
{
    internal interface IMessageGateHandlerBinder
    {
        void Bind(IMessageGateHandler handler);

        void Unbind(IMessageGateHandler handler);
    }
}
