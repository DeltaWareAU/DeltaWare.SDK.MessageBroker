using System;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Core
{
    public abstract class MessageInterceptor
    {
        public virtual void OnMessageReceived(Message message, Type messageType)
        {
        }
    }
}
