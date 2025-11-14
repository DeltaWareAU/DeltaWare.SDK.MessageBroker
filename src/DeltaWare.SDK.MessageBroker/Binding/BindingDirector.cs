using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;
using DeltaWare.SDK.MessageBroker.Core.Binding.Helpers;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeltaWare.SDK.MessageBroker.Core.Binding
{
    internal sealed class BindingDirector : IBindingDirector
    {
        private readonly Dictionary<Type, BindingDetails> _messageToBindingMap = new();

        private readonly Dictionary<BindingDetails, MessageHandlerBinding> _messageProcessors = new();

        public BindingDirector()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            DiscoverMessagesFromAssemblies(assemblies);
            DiscoverProcessorsFromAssemblies(assemblies);
        }

        public IEnumerable<IMessageHandlerBinding> GetHandlerBindings()
            => _messageProcessors.Select(map => map.Value);

        public IEnumerable<BindingDetails> GetMessageBindings()
            => _messageToBindingMap.Select(map => map.Value);

        public BindingDetails GetMessageBinding<T>() where T : class
            => _messageToBindingMap[typeof(T)];

        private void DiscoverProcessorsFromAssemblies(params Assembly[] assemblies)
            => BindingHelper
                .GetProcessorTypesFromAssemblies(assemblies)
                .ForEach(BindProcessor);

        private void DiscoverMessagesFromAssemblies(params Assembly[] assemblies)
            => BindingHelper
                .GetMessageTypesFromAssemblies(assemblies)
                .ForEach(BindMessage);

        private void BindProcessor(Type type)
        {
            Type? messageType = type.GetGenericArguments(typeof(MessageHandler<>)).FirstOrDefault();

            if (messageType is not { IsClass: true })
            {
                throw new Exception();
            }

            BindingDetails binding = _messageToBindingMap[messageType];

            if (type.TryGetCustomAttribute(out RoutingPatternAttribute? routingPattern))
            {
                binding = new BindingDetails(binding.Name, routingPattern!.Pattern, BrokerExchangeType.Topic);
            }

            if (!_messageProcessors.TryGetValue(binding, out MessageHandlerBinding? processorBinding))
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

            _messageToBindingMap.Add(type, bindingAttribute.BindingDetails);
        }
    }
}
