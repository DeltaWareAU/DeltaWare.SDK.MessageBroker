using System;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Core
{
    public interface IMessageInterceptor
    {
        void OnMessageReceived(Message message, Type messageType);
    }
}
