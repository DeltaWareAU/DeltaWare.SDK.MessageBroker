using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Broker;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class RabbitMqMessageBrokerOptions
    {
        public static void UseRabbitMQ(this IMessageBrokerOptions options, Action<RabbitMQ.Options.RabbitMqMessageBrokerOptions> optionsAction)
        {
            if (options is not MessageBrokerOptions brokerOptions)
            {
                throw new ArgumentException();
            }

            var rabbitMqOptions = new RabbitMQ.Options.RabbitMqMessageBrokerOptions();

            optionsAction.Invoke(rabbitMqOptions);

            brokerOptions.Services
                .AddSingleton<IRabbitMqMessageBrokerOptions>(rabbitMqOptions)
                .AddSingleton<IMessageBroker, RabbitMqMessageBroker>();
        }
    }
}
