using System;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Messages.Interception
{
    public abstract class MessageInterceptor
    {
        public virtual void OnMessageReceived(Message message, Type messageType)
        {
        }

        public virtual ValueTask OnMessageReceivedAsync(Message message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void OnMessageExecuting(Message message, Type messageType)
        {
        }

        public virtual ValueTask OnMessageExecutingAsync(Message message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void OnMessageExecuted(Message message, Type messageType)
        {
        }

        public virtual ValueTask OnMessageExecutedAsync(Message message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }

        public virtual void OnException(Message message, Type messageType, Exception exception)
        {
        }

        public virtual ValueTask OnExceptionAsync(Message message, Type messageType)
        {
            return ValueTask.CompletedTask;
        }
    }
}
