using DeltaWare.SDK.MessageBroker.Core.Messages;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider
{
    internal class MessageGateProvider : IMessageGateProvider
    {
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes(5);

        private readonly IMessageGateHandlerBinder _messageGateHandlerBinder;

        public MessageGateProvider(IMessageGateHandlerBinder messageGateHandlerBinder)
        {
            _messageGateHandlerBinder = messageGateHandlerBinder;
        }

        public MessageGate InitiateGate<TKey>(TKey key) where TKey : Message
        {
            return new MessageGateHandler<TKey>(key, _defaultTimeout, _messageGateHandlerBinder);
        }
    }
}
