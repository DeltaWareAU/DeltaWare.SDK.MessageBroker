using DeltaWare.SDK.MessageBroker.Core;
using DeltaWare.SDK.MessageBroker.Core.Messages;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor
{
    internal class MessageGateInterceptor : IMessageInterceptor, IMessageGateHandlerBinder
    {
        private readonly object _listLock = new object();

        private readonly List<IMessageGateHandler> _boundHandlers = new();

        public void OnMessageReceived(Message message, Type messageType)
        {
            lock (_listLock)
            {
                foreach (IMessageGateHandler boundHandler in _boundHandlers)
                {
                    boundHandler.TryOpen(message);
                }
            }
        }

        public void Bind(IMessageGateHandler handler)
        {
            lock (_listLock)
            {
                _boundHandlers.Add(handler);
            }
        }

        public void Unbind(IMessageGateHandler handler)
        {
            lock (_listLock)
            {
                _boundHandlers.Remove(handler);
            }
        }
    }
}
