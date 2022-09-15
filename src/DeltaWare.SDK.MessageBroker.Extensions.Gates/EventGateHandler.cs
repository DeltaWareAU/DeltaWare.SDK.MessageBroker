using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Processors;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    internal class EventGateHandler<TKey> : MessageHandler<TKey> where TKey : Message
    {
        public EventGate<TKey> Gate { get; }

        public EventGateHandler(TKey key, TimeSpan timeout)
        {
            Gate = new EventGate<TKey>(key, timeout);
        }

        protected override ValueTask ProcessAsync(TKey message)
        {
            Gate.TryUnlock(message);

            return ValueTask.CompletedTask;
        }
    }
}
