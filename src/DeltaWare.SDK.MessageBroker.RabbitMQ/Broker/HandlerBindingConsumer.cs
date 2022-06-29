using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Processors;
using DeltaWare.SDK.MessageBroker.Processors.Results;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Broker
{
    internal class HandlerBindingConsumer : DefaultBasicConsumer
    {
        private readonly IMessageHandlerManager _messageHandlerManager;

        private readonly IMessageHandlerBinding _binding;

        public IModel Channel { get; set; }

        public HandlerBindingConsumer(IMessageHandlerManager messageHandlerManager, IMessageHandlerBinding binding)
        {
            _messageHandlerManager = messageHandlerManager;
            _binding = binding;
        }

        public override async void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            string message = Encoding.UTF8.GetString(body.ToArray());

            IMessageHandlerResults results = await _messageHandlerManager.HandleMessageAsync(_binding, message);

            if (results.WasSuccessful)
            {
                Channel.BasicAck(deliveryTag, false);
            }
            else
            {
                Channel.BasicNack(deliveryTag, false, results.Retry);
            }
        }
    }
}
