using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using DeltaWare.SDK.MessageBroker.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    internal class EventGateHandler<TKey> : EventGate<TKey>, IEventGateHandler where TKey : Message
    {
        private readonly IEventGateHandlerBinder _binder;

        public EventGateHandler(TKey key, TimeSpan timeout, IEventGateHandlerBinder binder) : base(key, timeout)
        {
            _binder = binder;
            _binder.Bind(this);
        }

        public void TryOpen(Message message)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }

            if (message is TKey key)
            {
                base.TryOpen(key);
            }
        }

        private bool _disposed;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _binder.Unbind(this);
            }

            _disposed = true;
        }
    }
}
