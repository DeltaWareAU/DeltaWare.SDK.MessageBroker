using System;
using System.Collections.Generic;
using System.Linq;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding;

namespace DeltaWare.SDK.MessageBroker.Binding
{
    public class MessageHandlerBinding
    {
        private readonly HashSet<Type> _processorTypes = new();

        public BindingDetails Details { get; }
        public Type MessageType { get; }
        public IReadOnlyList<Type> HandlerTypes => _processorTypes.ToList();

        public MessageHandlerBinding(BindingDetails bindingDetails, Type messageType)
        {
            Details = bindingDetails;
            MessageType = messageType;
        }

        public bool AddProcessor(Type processorType)
        {
            return _processorTypes.Add(processorType);
        }
    }
}
