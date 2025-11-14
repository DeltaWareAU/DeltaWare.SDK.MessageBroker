using Azure.Messaging.ServiceBus;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;
using DeltaWare.SDK.MessageBroker.Core.Broker;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
using DeltaWare.SDK.MessageBroker.Core.Handlers.Results;
using DeltaWare.SDK.MessageBroker.Core.Messages.Properties;
using DeltaWare.SDK.MessageBroker.Core.Messages.Serialization;
using DeltaWare.SDK.MessageBroker.ServiceBus.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Core.Binding;

namespace DeltaWare.SDK.MessageBroker.ServiceBus.Broker
{
    internal sealed class ServiceBusMessageBroker : IMessageBroker, IAsyncDisposable
    {
        private readonly ServiceBusClient _serviceBusClient;

        private readonly IBindingDirector _bindingDirector;

        private readonly IMessageHandlerManager _messageHandlerManager;

        private readonly IMessageSerializer _messageSerializer;

        private readonly Dictionary<BindingDetails, ServiceBusSender> _boundSenders = new();

        private IReadOnlyDictionary<IMessageHandlerBinding, ServiceBusProcessor> _handlerBindings;

        private readonly IPropertiesBuilder _propertiesBuilder;

        private readonly ILogger _logger;

        public bool Initiated { get; private set; }
        public bool IsListening { get; private set; }
        public bool IsProcessing => _handlerBindings?.Values.Any(p => p.IsProcessing) ?? false;

        public ServiceBusMessageBroker(ILogger<ServiceBusMessageBroker> logger, ServiceBusMessageBrokerOptions options, IMessageHandlerManager messageHandlerManager, IMessageSerializer messageSerializer, IBindingDirector bindingDirector, IPropertiesBuilder propertiesBuilder)
        {
            _logger = logger;
            _serviceBusClient = new ServiceBusClient(options.ConnectionString);
            _messageHandlerManager = messageHandlerManager;
            _messageSerializer = messageSerializer;
            _bindingDirector = bindingDirector;
            _propertiesBuilder = propertiesBuilder;
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            BindingDetails bindingDetails = _bindingDirector.GetMessageBinding<TMessage>();

            if (!_boundSenders.TryGetValue(bindingDetails, out ServiceBusSender sender))
            {
                sender = _serviceBusClient.CreateSender(bindingDetails.Name);

                _boundSenders.Add(bindingDetails, sender);
            }

            ServiceBusMessage serviceBusMessage = CreateServiceBusMessage(message);

            await sender.SendMessageAsync(serviceBusMessage, cancellationToken);
        }

        public ValueTask InitiateBindingsAsync(CancellationToken cancellationToken)
        {
            if (Initiated)
            {
                throw new InvalidOperationException("Bindings have already been initiated.");
            }

            Dictionary<IMessageHandlerBinding, ServiceBusProcessor> handlerBindings = new Dictionary<IMessageHandlerBinding, ServiceBusProcessor>();

            foreach (IMessageHandlerBinding binding in _bindingDirector.GetHandlerBindings())
            {
                ServiceBusProcessor processor = InitiateBinding(binding);

                handlerBindings.Add(binding, processor);
            }

            _handlerBindings = handlerBindings;

            Initiated = true;

            return ValueTask.CompletedTask;
        }

        private ServiceBusProcessor InitiateBinding(IMessageHandlerBinding binding)
        {
            ServiceBusProcessor processor = binding.Details.ExchangeType switch
            {
                BrokerExchangeType.Fanout => _serviceBusClient.CreateProcessor(binding.Details.Name),
                BrokerExchangeType.Direct => _serviceBusClient.CreateProcessor(binding.Details.Name),
                BrokerExchangeType.Topic => _serviceBusClient.CreateProcessor(binding.Details.Name, binding.Details.RoutingPattern),
                _ => throw new ArgumentOutOfRangeException()
            };

            processor.ProcessMessageAsync += args => OnMessageAsync(args, binding, args.CancellationToken);
            processor.ProcessErrorAsync += args => OnErrorsAsync(args, binding);

            return processor;
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken)
        {
            if (IsListening)
            {
                return;
            }

            IsListening = true;

            foreach (ServiceBusProcessor processor in _handlerBindings.Values)
            {
                await processor.StartProcessingAsync(cancellationToken);
            }
        }

        public async Task StopListeningAsync(CancellationToken cancellationToken)
        {
            if (!IsListening)
            {
                return;
            }

            IsListening = false;

            foreach (ServiceBusProcessor processor in _handlerBindings.Values)
            {
                await processor.StopProcessingAsync(cancellationToken);
            }
        }

        private ServiceBusMessage CreateServiceBusMessage<TMessage>(TMessage message) where TMessage : class
        {
            string messageBody = _messageSerializer.Serialize(message);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(messageBody);

            foreach (KeyValuePair<string, object> property in _propertiesBuilder.BuildProperties(message))
            {
                serviceBusMessage.ApplicationProperties.Add(property);
            }

            return serviceBusMessage;
        }

        private async Task OnMessageAsync(ProcessMessageEventArgs args, IMessageHandlerBinding binding, CancellationToken cancellationToken)
        {
            MessageHandlerResults results = await _messageHandlerManager.HandleMessageAsync(binding, args.Message.Body.ToString(), cancellationToken);

            if (results.WasSuccessful)
            {
                await args.CompleteMessageAsync(args.Message, cancellationToken);

                return;
            }

            if (results.Retry)
            {
                return;
            }

            await args.DeadLetterMessageAsync(args.Message, cancellationToken: cancellationToken);
        }

        private Task OnErrorsAsync(ProcessErrorEventArgs args, IMessageHandlerBinding binding)
        {
            _logger.LogError(args.Exception, "Failed to process {messageName}", binding.Details.Name);

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            foreach (ServiceBusProcessor processor in _handlerBindings.Values)
            {
                await processor.DisposeAsync();
            }

            foreach (ServiceBusSender sender in _boundSenders.Values)
            {
                await sender.DisposeAsync();
            }

            await _serviceBusClient.DisposeAsync();
        }
    }
}
