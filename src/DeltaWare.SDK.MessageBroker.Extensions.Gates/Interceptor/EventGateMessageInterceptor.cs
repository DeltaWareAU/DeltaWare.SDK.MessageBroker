using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor
{
    internal class EventGateMessageInterceptor : IMessageInterceptor, IEventGateHandlerBinder
    {
        private readonly object _listLock = new object();

        private readonly List<IEventGateHandler> _boundHandlers = new();

        public void OnMessageReceived(Message message, Type messageType)
        {
            lock (_listLock)
            {
                foreach (IEventGateHandler boundHandler in _boundHandlers)
                {
                    boundHandler.TryOpen(message);
                }
            }
        }

        public void Bind(IEventGateHandler handler)
        {
            lock (_listLock)
            {
                _boundHandlers.Add(handler);
            }
        }

        public void Unbind(IEventGateHandler handler)
        {
            lock (_listLock)
            {
                _boundHandlers.Remove(handler);
            }
        }
    }
}
