using DeltaWare.SDK.MessageBroker.RabbitMQ.Broker;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Options;
using System;
using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.Options;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class RabbitMqOptions
    {
        public static void UseRabbitMQ(this MessageBrokerOptions options, Action<RabbitMqMessageBrokerOptions> optionsAction)
        {
            var rabbitMqOptions = new RabbitMqMessageBrokerOptions();

            optionsAction.Invoke(rabbitMqOptions);

            options.Services
                .AddSingleton(rabbitMqOptions)
                .AddSingleton<IMessageBroker, RabbitMqMessageBroker>();
        }
    }
}
