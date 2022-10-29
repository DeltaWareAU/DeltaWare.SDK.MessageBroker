﻿using Azure.Messaging.ServiceBus;
using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Binding.Enums;
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

namespace DeltaWare.SDK.MessageBroker.ServiceBus.Broker
{
    internal class ServiceBusMessageBroker : IMessageBroker, IAsyncDisposable
    {
        private readonly ServiceBusClient _serviceBusClient;

        private readonly IBindingDirector _bindingDirector;

        private readonly IMessageHandlerManager _messageHandlerManager;

        private readonly IMessageSerializer _messageSerializer;

        private readonly Dictionary<IBindingDetails, ServiceBusSender> _boundSenders = new();

        private IReadOnlyDictionary<IMessageHandlerBinding, ServiceBusProcessor> _handlerBindings;

        private readonly IPropertiesBuilder _propertiesBuilder;

        private readonly ILogger _logger;

        public bool Initiated { get; private set; }
        public bool IsListening { get; private set; }
        public bool IsProcessing => _handlerBindings?.Values.Any(p => p.IsProcessing) ?? false;

        public ServiceBusMessageBroker(ILogger<ServiceBusMessageBroker> logger, IServiceBusMessageBrokerOptions options, IMessageHandlerManager messageHandlerManager, IMessageSerializer messageSerializer, IBindingDirector bindingDirector, IPropertiesBuilder propertiesBuilder)
        {
            _logger = logger;
            _serviceBusClient = new ServiceBusClient(options.ConnectionString);
            _messageHandlerManager = messageHandlerManager;
            _messageSerializer = messageSerializer;
            _bindingDirector = bindingDirector;
            _propertiesBuilder = propertiesBuilder;
        }

        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            IBindingDetails bindingDetails = _bindingDirector.GetMessageBinding<TMessage>();

            if (!_boundSenders.TryGetValue(bindingDetails, out ServiceBusSender sender))
            {
                sender = _serviceBusClient.CreateSender(bindingDetails.Name);

                _boundSenders.Add(bindingDetails, sender);
            }

            ServiceBusMessage serviceBusMessage = CreateServiceBusMessage(message);

            return sender.SendMessageAsync(serviceBusMessage, cancellationToken);
        }

        public void InitiateBindings()
        {
            if (Initiated)
            {
                return;
            }

            Dictionary<IMessageHandlerBinding, ServiceBusProcessor> handlerBindings = new Dictionary<IMessageHandlerBinding, ServiceBusProcessor>();

            foreach (IMessageHandlerBinding binding in _bindingDirector.GetHandlerBindings())
            {
                ServiceBusProcessor processor;

                switch (binding.Details.ExchangeType)
                {
                    case BrokerExchangeType.Fanout:
                    case BrokerExchangeType.Direct:
                        processor = _serviceBusClient.CreateProcessor(binding.Details.Name);
                        break;
                    case BrokerExchangeType.Topic:
                        processor = _serviceBusClient.CreateProcessor(binding.Details.Name, binding.Details.RoutingPattern);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                processor.ProcessMessageAsync += args => OnMessageAsync(args, binding);
                processor.ProcessErrorAsync += args => OnErrorsAsync(args, binding);

                handlerBindings.Add(binding, processor);
            }

            _handlerBindings = handlerBindings;

            Initiated = true;
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

        private async Task OnMessageAsync(ProcessMessageEventArgs args, IMessageHandlerBinding binding)
        {
            IMessageHandlerResults results = await _messageHandlerManager.HandleMessageAsync(binding, args.Message.Body.ToString());

            if (results.WasSuccessful)
            {
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                if (!results.Retry)
                {
                    await args.DeadLetterMessageAsync(args.Message);
                }
            }
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
