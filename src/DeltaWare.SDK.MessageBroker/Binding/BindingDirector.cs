using DeltaWare.SDK.MessageBroker.Binding.Attributes;
using DeltaWare.SDK.MessageBroker.Binding.Enums;
using DeltaWare.SDK.MessageBroker.Binding.Helpers;
using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.MessageBroker.Binding
{
    public class BindingDirector : IBindingDirector
    {
        private readonly Dictionary<Type, IBindingDetails> _messageToBindingMap = new();

        private readonly Dictionary<IBindingDetails, MessageHandlerBinding> _messageProcessors = new();

        public BindingDirector()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            DiscoverMessagesFromAssemblies(assemblies);
            DiscoverProcessorsFromAssemblies(assemblies);
        }

        public IEnumerable<IMessageHandlerBinding> GetHandlerBindings() => _messageProcessors.Select(map => map.Value);
        public IEnumerable<IBindingDetails> GetMessageBindings() => _messageToBindingMap.Select(map => map.Value);

        public IBindingDetails GetMessageBinding<T>() where T : Message => _messageToBindingMap[typeof(T)];

        private void DiscoverProcessorsFromAssemblies(params Assembly[] assemblies)
        {
            BindingHelper
                .GetProcessorTypesFromAssemblies(assemblies)
                .ForEach(BindProcessor);
        }



        private void DiscoverMessagesFromAssemblies(params Assembly[] assemblies)
        {
            BindingHelper
                .GetMessageTypesFromAssemblies(assemblies)
                .ForEach(BindMessage);
        }

        #region Binding

        private void BindProcessor(Type type)
        {
            Type? messageType = type.GetGenericArguments(typeof(MessageHandler<>)).FirstOrDefault();

            if (messageType == null)
            {
                throw new Exception();
            }

            if (!messageType.IsSubclassOf<Message>())
            {
                throw new Exception();
            }

            IBindingDetails binding = _messageToBindingMap[messageType];

            if (type.TryGetCustomAttribute(out RoutingPatternAttribute routingPattern))
            {
                binding = new BindingDetails
                {
                    ExchangeType = BrokerExchangeType.Topic,
                    Name = binding.Name,
                    RoutingPattern = routingPattern.Pattern
                };
            }

            if (!_messageProcessors.TryGetValue(binding, out MessageHandlerBinding processorBinding))
            {
                processorBinding = new MessageHandlerBinding(binding, messageType);

                _messageProcessors.Add(binding, processorBinding);
            }

            processorBinding.AddProcessor(type);
        }

        private void BindMessage(Type type)
        {
            var bindingAttribute = type.GetCustomAttribute<MessageBrokerBindingAttribute>();

            if (bindingAttribute == null)
            {
                throw new Exception($"A message ({type.Name}) does not have a Binding Attribute Applied.");
            }

            IBindingDetails bindingDetails = bindingAttribute.GetBindingDetails();

            _messageToBindingMap.Add(type, bindingDetails);
        }

        #endregion
    }
}
