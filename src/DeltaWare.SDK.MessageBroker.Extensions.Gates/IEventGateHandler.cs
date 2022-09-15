using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    internal interface IEventGateHandler
    {
        void TryOpen(Message message);
    }
}
