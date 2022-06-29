using DeltaWare.SDK.MessageBroker.Messages;
using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Binding
{
    public interface IBindingDirector
    {
        IEnumerable<IMessageHandlerBinding> GetHandlerBindings();

        IEnumerable<IBindingDetails> GetMessageBindings();

        IBindingDetails GetMessageBinding<T>() where T : Message;
    }
}
