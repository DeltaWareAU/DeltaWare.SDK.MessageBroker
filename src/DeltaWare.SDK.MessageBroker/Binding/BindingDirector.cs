using DeltaWare.SDK.MessageBroker.Binding.Attributes;
using DeltaWare.SDK.MessageBroker.Binding.Enums;
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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            DiscoverMessagesFromAssemblies(assemblies);
            DiscoverProcessorsFromAssemblies(assemblies);
        }

        public IEnumerable<IMessageHandlerBinding> GetHandlerBindings() => _messageProcessors.Select(map => map.Value);
        public IEnumerable<IBindingDetails> GetMessageBindings() => _messageToBindingMap.Select(map => map.Value);

        public IBindingDetails GetMessageBinding<T>() where T : Message => _messageToBindingMap[typeof(T)];

        private void DiscoverProcessorsFromAssemblies(params Assembly[] assemblies)
        {
            foreach (Type processorType in GetProcessorTypesFromAssemblies(assemblies))
            {
                Type? messageType = processorType.GetGenericArguments(typeof(MessageHandler<>)).FirstOrDefault();

                if (messageType == null)
                {
                    throw new Exception();
                }

                if (!messageType.IsSubclassOf<Message>())
                {
                    throw new Exception();
                }

                IBindingDetails binding = _messageToBindingMap[messageType];

                if (processorType.TryGetCustomAttribute(out RoutingPatternAttribute routingPattern))
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

                processorBinding.AddProcessor(processorType);
            }
        }

        private void DiscoverMessagesFromAssemblies(params Assembly[] assemblies)
        {
            foreach (Type messageType in GetMessageTypesFromAssemblies(assemblies))
            {
                var bindingAttribute = messageType.GetCustomAttribute<MessageBrokerBindingAttribute>();

                if (bindingAttribute == null)
                {
                    throw new Exception($"A message ({messageType.Name}) does not have a Binding Attribute Applied.");
                }

                IBindingDetails bindingDetails = bindingAttribute.GetBindingDetails();

                _messageToBindingMap.Add(messageType, bindingDetails);
            }
        }

        private IEnumerable<Type> GetProcessorTypesFromAssemblies(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsSubclassOfRawGeneric(typeof(MessageHandler<>))));

        }

        private IEnumerable<Type> GetMessageTypesFromAssemblies(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsSubclassOf<Message>()));

        }
    }
}
