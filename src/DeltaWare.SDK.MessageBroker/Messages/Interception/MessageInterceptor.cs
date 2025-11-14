using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Messages.Interception
{
    public abstract class MessageInterceptor
    {
        public virtual ValueTask OnMessageReceivedAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual ValueTask OnMessageExecutingAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual ValueTask OnMessageExecutedAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual ValueTask OnExceptionAsync(object message, Type messageType, Exception exception)
        {
            return ValueTask.CompletedTask;
        }
    }
}
