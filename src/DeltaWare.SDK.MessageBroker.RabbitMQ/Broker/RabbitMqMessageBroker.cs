using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.Handlers;
using DeltaWare.SDK.MessageBroker.Messages.Properties;
using DeltaWare.SDK.MessageBroker.Messages.Serialization;

namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Broker
{
    internal sealed class RabbitMqMessageBroker : IMessageBroker, IAsyncDisposable
    {
        private readonly RabbitMqMessageBrokerOptions _options;

        private readonly IBindingDirector _bindingDirector;

        private readonly IMessageHandlerManager _messageHandlerManager;

        private readonly IMessageSerializer _messageSerializer;

        private readonly IPropertiesBuilder _propertiesBuilder;

        private readonly ILogger _logger;

        private IConnection? _connection;

        private IChannel _channel;

        private IReadOnlyDictionary<MessageHandlerBinding, HandlerBindingConsumer> _handlerBindings;

        public bool Initiated { get; private set; }
        public bool IsListening { get; private set; }
        public bool IsProcessing => _handlerBindings.Values.Any(h => h.IsRunning);

        public RabbitMqMessageBroker(ILogger<RabbitMqMessageBroker> logger, RabbitMqMessageBrokerOptions options, IBindingDirector bindingDirector, IMessageHandlerManager messageHandlerManager, IMessageSerializer messageSerializer, IPropertiesBuilder propertiesBuilder)
        {
            _logger = logger;
            _options = options;
            _bindingDirector = bindingDirector;
            _messageHandlerManager = messageHandlerManager;
            _messageSerializer = messageSerializer;
            _propertiesBuilder = propertiesBuilder;
        }

        public async Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            if (_channel is null)
            {
                throw new InvalidOperationException("Channel has not been created.");
            }

            var binding = _bindingDirector.GetMessageBinding<TMessage>();

            var serializedMessage = _messageSerializer.Serialize(message);

            var properties = new BasicProperties
            {
                Headers = new Dictionary<string, object>()
            };

            foreach (KeyValuePair<string, object> property in _propertiesBuilder.BuildProperties(message))
            {
                properties.Headers[property.Key] = property.Value;
            }

            var messageBuffer = Encoding.UTF8.GetBytes(serializedMessage);

            await _channel.BasicPublishAsync(
                exchange: binding.Name,
                routingKey: binding.RoutingPattern ?? string.Empty,
                mandatory: false,
                basicProperties: properties,
                body: messageBuffer,
                cancellationToken: cancellationToken);
        }

        public async ValueTask InitiateBindingsAsync(CancellationToken cancellationToken)
        {
            if (Initiated)
            {
                throw new InvalidOperationException("Bindings have already been initiated.");
            }

            Initiated = true;

            await OpenConnectionAsync(_options, cancellationToken);

            var handlerBindings = new Dictionary<MessageHandlerBinding, HandlerBindingConsumer>();

            foreach (var binding in _bindingDirector.GetHandlerBindings())
            {
                var consumer = new HandlerBindingConsumer(_channel, _messageHandlerManager, binding);

                handlerBindings.Add(binding, consumer);
            }

            _handlerBindings = handlerBindings;
        }

        public async Task StopListeningAsync(CancellationToken cancellationToken = default)
        {
            if (!IsListening)
            {
                return;
            }

            IsListening = false;

            await _channel.CloseAsync(cancellationToken);
            await _channel.DisposeAsync();

            _channel = null;

            _logger.LogInformation("Message Broker has Stopped Listening for Incoming Messages.");
        }

        public async Task StartListeningAsync(CancellationToken cancellationToken = default)
        {
            if (!Initiated)
            {
                throw new InvalidOperationException("Bindings have not been initiated.");
            }

            if (IsListening)
            {
                throw new InvalidOperationException("Broker is already Listening");
            }

            IsListening = true;

            _channel = await _connection!.CreateChannelAsync(cancellationToken: cancellationToken);

            foreach ((var binding, var consumer) in _handlerBindings)
            {
                var queueName = binding.Details.ExchangeType switch
                {
                    BrokerExchangeType.Fanout => binding.Details.Name,
                    BrokerExchangeType.Direct => binding.Details.Name,
                    BrokerExchangeType.Topic => binding.Details.RoutingPattern!, // assuming this really is your queue
                    _ => throw new ArgumentOutOfRangeException()
                };

                await _channel.BasicConsumeAsync(
                    queue: queueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: cancellationToken);
            }

            _logger.LogInformation("Rabbit MQ Message Broker has Started Listening for Incoming Messages.");
        }

        private async Task OpenConnectionAsync(RabbitMqMessageBrokerOptions options, CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory
            {
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = options.VirtualHost,
                HostName = options.HostName,
                Port = options.Port
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _connection.ConnectionShutdownAsync += OnConnectionShutdownAsync;
        }

        private async Task OnConnectionShutdownAsync(object sender, ShutdownEventArgs @event)
        {
            _logger.LogWarning("Rabbit MQ Host offline, Message Received: {message}", @event.ReplyText);

            await StopListeningAsync();
        }

        #region IAsyncDisposable

        private bool _disposed;

        public ValueTask DisposeAsync()
        {
            return DisposeAsync(true);
        }

        private async ValueTask DisposeAsync(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (IsListening)
                {
                    await StopListeningAsync();
                }

                if (_connection is not null)
                {
                    await _connection.CloseAsync();
                    await _connection.DisposeAsync();
                }
            }

            _disposed = true;
        }

        #endregion
    }
}
