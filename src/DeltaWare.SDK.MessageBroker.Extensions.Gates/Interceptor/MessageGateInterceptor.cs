using DeltaWare.SDK.MessageBroker.Core.Messages.Interception;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler;
using System;
using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor
{
    internal class MessageGateInterceptor : MessageInterceptor, IMessageGateHandlerBinder
    {
        private readonly object _listLock = new();

        private readonly List<IMessageGateHandler> _boundHandlers = new();

        public override void OnMessageReceived(object message, Type messageType)
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
