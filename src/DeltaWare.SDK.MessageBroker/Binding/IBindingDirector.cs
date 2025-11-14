using System.Collections.Generic;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding;

namespace DeltaWare.SDK.MessageBroker.Core.Binding
{
    public interface IBindingDirector
    {
        IEnumerable<MessageHandlerBinding> GetHandlerBindings();

        IEnumerable<BindingDetails> GetMessageBindings();

        BindingDetails GetMessageBinding<T>() where T : class;
    }
}
