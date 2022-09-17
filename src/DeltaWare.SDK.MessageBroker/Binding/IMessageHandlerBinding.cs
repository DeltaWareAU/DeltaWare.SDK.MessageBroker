using System;
using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Core.Binding
{
    public interface IMessageHandlerBinding
    {
        IBindingDetails Details { get; }
        IReadOnlyList<Type> HandlerTypes { get; }
        Type MessageType { get; }
    }
}
