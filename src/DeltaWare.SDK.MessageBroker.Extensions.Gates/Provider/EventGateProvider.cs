using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider
{
    internal class EventGateProvider : IEventGateProvider
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(5);

        private readonly IEventGateHandlerBinder _eventGateHandlerBinder;

        public EventGateProvider(IEventGateHandlerBinder eventGateHandlerBinder)
        {
            _eventGateHandlerBinder = eventGateHandlerBinder;
        }

        public EventGate GetGate<TKey>(TKey key) where TKey : Message
        {
            return new EventGateHandler<TKey>(key, _defaultTimeout, _eventGateHandlerBinder);
        }
    }
}
