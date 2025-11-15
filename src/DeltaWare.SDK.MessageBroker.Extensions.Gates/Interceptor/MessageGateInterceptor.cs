using DeltaWare.SDK.MessageBroker.Extensions.Gates.Handler;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Messages.Interception;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor
{
    internal class MessageGateInterceptor : MessageInterceptor, IMessageGateHandlerBinder
    {
        private static readonly Lock Lock = new();

        private readonly List<IMessageGateHandler> _boundHandlers = new();

        public override ValueTask OnMessageReceivedAsync(object message, Type messageType)
        {
            lock (Lock)
            {
                foreach (IMessageGateHandler boundHandler in _boundHandlers)
                {
                    boundHandler.TryOpen(message);
                }
            }

            return ValueTask.CompletedTask;
        }

        public void Bind(IMessageGateHandler handler)
        {
            lock (Lock)
            {
                _boundHandlers.Add(handler);
            }
        }

        public void Unbind(IMessageGateHandler handler)
        {
            lock (Lock)
            {
                _boundHandlers.Remove(handler);
            }
        }
    }
}
