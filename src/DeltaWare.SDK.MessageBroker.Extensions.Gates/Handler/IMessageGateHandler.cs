using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler
{
    internal interface IMessageGateHandler
    {
        void TryOpen(Message message);
    }
}
