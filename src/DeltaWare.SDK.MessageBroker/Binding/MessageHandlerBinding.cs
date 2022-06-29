using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.SDK.MessageBroker.Binding
{
    internal class MessageHandlerBinding : IMessageHandlerBinding
    {
        private readonly HashSet<Type> _processorTypes = new();

        public IBindingDetails Details { get; }
        public IReadOnlyList<Type> HandlerTypes => _processorTypes.ToList();
        public Type MessageType { get; }

        public MessageHandlerBinding(IBindingDetails bindingDetails, Type messageType)
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
