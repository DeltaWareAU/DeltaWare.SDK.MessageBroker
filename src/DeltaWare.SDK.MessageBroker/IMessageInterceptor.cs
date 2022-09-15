using DeltaWare.SDK.MessageBroker.Messages;
using System;

namespace DeltaWare.SDK.MessageBroker
{
    public interface IMessageInterceptor
    {
        void OnMessageReceived(Message message, Type messageType);
    }
}
