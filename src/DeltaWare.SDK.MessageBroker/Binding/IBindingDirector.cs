using System.Collections.Generic;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Core.Binding
{
    public interface IBindingDirector
    {
        IEnumerable<IMessageHandlerBinding> GetHandlerBindings();

        IEnumerable<IBindingDetails> GetMessageBindings();

        IBindingDetails GetMessageBinding<T>() where T : Message;
    }
}
