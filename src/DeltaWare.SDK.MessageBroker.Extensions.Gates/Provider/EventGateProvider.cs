using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider
{
    internal class EventGateProvider : IEventGateProvider
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(5);

        public EventGate GetGate<TKey>(TKey key) where TKey : Message
        {
            EventGateHandler<TKey> gateHandler = new EventGateHandler<TKey>(key, _defaultTimeout);

            return gateHandler.Gate;
        }
    }
}
