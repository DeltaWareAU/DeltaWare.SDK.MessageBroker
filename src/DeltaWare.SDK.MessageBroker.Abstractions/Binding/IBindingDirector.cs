using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding
{
    public interface IBindingDirector
    {
        IEnumerable<IMessageHandlerBinding> GetHandlerBindings();

        IEnumerable<IBindingDetails> GetMessageBindings();

        IBindingDetails GetMessageBinding<T>() where T : class;
    }
}
