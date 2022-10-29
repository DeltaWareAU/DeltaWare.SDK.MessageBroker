using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Messages.Interception
{
    public abstract class MessageInterceptor
    {
        public virtual void OnMessageReceived(object message, Type messageType)
        {
        }

        public virtual ValueTask OnMessageReceivedAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void OnMessageExecuting(object message, Type messageType)
        {
        }

        public virtual ValueTask OnMessageExecutingAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void OnMessageExecuted(object message, Type messageType)
        {
        }

        public virtual ValueTask OnMessageExecutedAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void OnException(object message, Type messageType, Exception exception)
        {
        }

        public virtual ValueTask OnExceptionAsync(object message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }
    }
}
