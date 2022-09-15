using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider
{
    public interface IEventGateProvider
    {
        EventGate GetGate<TKey>(TKey key) where TKey : Message;
    }
}
