using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using System;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler
{
    internal class MessageGateHandler<TKey> : MessageGate<TKey>, IMessageGateHandler where TKey : class
    {
        private readonly IMessageGateHandlerBinder _binder;

        public MessageGateHandler(TKey key, TimeSpan timeout, IMessageGateHandlerBinder binder) : base(key, timeout)
        {
            _binder = binder;
            _binder.Bind(this);
        }

        public void TryOpen(object message)
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
