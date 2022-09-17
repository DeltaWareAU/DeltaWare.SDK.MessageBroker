using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler
{
    internal interface IMessageGateHandler
    {
        void TryOpen(Message message);
    }
}
