using DeltaWare.SDK.MessageBroker.Handlers;
using DeltaWare.SDK.MessageBroker.Handlers.Results;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Binding;

namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Broker
{
    internal sealed class HandlerBindingConsumer : AsyncEventingBasicConsumer
    {
        private readonly IMessageHandlerManager _messageHandlerManager;

        private readonly MessageHandlerBinding _binding;

        public HandlerBindingConsumer(IChannel channel, IMessageHandlerManager messageHandlerManager, MessageHandlerBinding binding) : base(channel)
        {
            _messageHandlerManager = messageHandlerManager;
            _binding = binding;
        }

        public override async Task HandleBasicDeliverAsync(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IReadOnlyBasicProperties properties, ReadOnlyMemory<byte> body, CancellationToken cancellationToken)
        {
            string message = Encoding.UTF8.GetString(body.ToArray());

            var results = await _messageHandlerManager.HandleMessageAsync(_binding, message, cancellationToken);

            if (results.WasSuccessful)
            {
                await Channel.BasicAckAsync(deliveryTag, false, cancellationToken);
            }
            else
            {
                await Channel.BasicNackAsync(deliveryTag, false, results.Retry, cancellationToken);
            }
        }
    }
}
