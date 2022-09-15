namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor
{
    internal interface IEventGateHandlerBinder
    {
        void Bind(IEventGateHandler handler);

        void Unbind(IEventGateHandler handler);
    }
}
