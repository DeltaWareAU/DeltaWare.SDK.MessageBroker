namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler
{
    internal interface IMessageGateHandler
    {
        void TryOpen(object message);
    }
}
