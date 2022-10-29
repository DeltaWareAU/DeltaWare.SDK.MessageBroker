using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Binding.Enums;
using DeltaWare.SDK.MessageBroker.Core.Broker;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
using DeltaWare.SDK.MessageBroker.Core.Messages.Properties;
using DeltaWare.SDK.MessageBroker.Core.Messages.Serialization;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Broker
{
    internal class RabbitMqMessageBroker : IMessageBroker, IAsyncDisposable
    {
        private readonly IRabbitMqMessageBrokerOptions _options;

        private readonly IConnection _connection;

        private readonly IBindingDirector _bindingDirector;

        private readonly IMessageHandlerManager _messageHandlerManager;

        private readonly IMessageSerializer _messageSerializer;

        private readonly IPropertiesBuilder _propertiesBuilder;

        private readonly ILogger _logger;

        private IModel _channel;

        private IReadOnlyDictionary<IMessageHandlerBinding, HandlerBindingConsumer> _handlerBindings;

        public bool Initiated { get; private set; }
        public bool IsListening { get; private set; }
        public bool IsProcessing => _handlerBindings.Values.Any(h => h.IsRunning);

        public RabbitMqMessageBroker(ILogger<RabbitMqMessageBroker> logger, IRabbitMqMessageBrokerOptions options, IBindingDirector bindingDirector, IMessageHandlerManager messageHandlerManager, IMessageSerializer messageSerializer, IPropertiesBuilder propertiesBuilder)
        {
            _logger = logger;
            _options = options;
            _bindingDirector = bindingDirector;
            _messageHandlerManager = messageHandlerManager;
            _messageSerializer = messageSerializer;
            _propertiesBuilder = propertiesBuilder;
            _connection = OpenConnection(options);

            _connection.ConnectionShutdown += OnShutdown;
        }

        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : class
        {
            return Task.Run(() =>
            {
                IBindingDetails binding = _bindingDirector.GetMessageBinding<TMessage>();

                string serializedMessage = _messageSerializer.Serialize(message);

                IBasicProperties properties = _channel.CreateBasicProperties();

                foreach (KeyValuePair<string, object> property in _propertiesBuilder.BuildProperties(message))
                {
                    properties.Headers.Add(property);
                }

                byte[] messageBuffer = Encoding.UTF8.GetBytes(serializedMessage);

                _channel.BasicPublish(binding.Name, binding.RoutingPattern ?? string.Empty, properties, messageBuffer);
            }, cancellationToken);
        }

        public void InitiateBindings()
        {
            if (Initiated)
            {
                return;
            }

            Initiated = true;

            Dictionary<IMessageHandlerBinding, HandlerBindingConsumer> handlerBindings = new Dictionary<IMessageHandlerBinding, HandlerBindingConsumer>();

            var bindings = _bindingDirector.GetHandlerBindings();

            foreach (IMessageHandlerBinding binding in bindings)
            {
                HandlerBindingConsumer consumer = new HandlerBindingConsumer(_messageHandlerManager, binding);

                handlerBindings.Add(binding, consumer);
            }

            _handlerBindings = handlerBindings;
        }

        public Task StopListeningAsync(CancellationToken cancellationToken = default)
        {
            if (!IsListening)
            {
                return Task.CompletedTask;
            }

            IsListening = false;

            _channel.Close();
            _channel.Dispose();
            _channel = null;

            _logger.LogInformation("Message Broker has Stopped Listening for Incoming Messages.");

            return Task.CompletedTask;
        }

        public Task StartListeningAsync(CancellationToken cancellationToken = default)
        {
            if (IsListening)
            {
                return Task.CompletedTask;
            }

            IsListening = true;

            _channel = _connection.CreateModel();

            foreach ((IMessageHandlerBinding binding, HandlerBindingConsumer consumer) in _handlerBindings)
            {
                consumer.Channel = _channel;

                switch (binding.Details.ExchangeType)
                {
                    case BrokerExchangeType.Fanout:
                    case BrokerExchangeType.Direct:
                        _channel.BasicConsume(binding.Details.Name, false, consumer);
                        break;
                    case BrokerExchangeType.Topic:
                        _channel.BasicConsume(binding.Details.RoutingPattern, false, consumer);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            _logger.LogInformation("Message Broker has Started Listening for Incoming Messages.");

            return Task.CompletedTask;
        }

        private IConnection OpenConnection(IRabbitMqMessageBrokerOptions options)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = options.UserName,
                Password = options.Password,
                VirtualHost = options.VirtualHost,
                HostName = options.HostName,
                Port = options.Port
            };

            return factory.CreateConnection();
        }

        private async void OnShutdown(object sender, ShutdownEventArgs args)
        {
            _logger.LogWarning("Rabbit MQ Host offline, Message Received: {message}", args.ReplyText);

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

                _connection?.Close();
                _connection?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
