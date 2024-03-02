using DeltaWare.SDK.MessageBroker.Abstractions.Broker;
using DeltaWare.SDK.MessageBroker.Core.Options;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Broker;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class RabbitMqOptions
    {
        public static void UseRabbitMQ(this IMessageBrokerOptions options, Action<RabbitMqMessageBrokerOptions> optionsAction)
        {
            if (options is not MessageBrokerOptions brokerOptions)
            {
                throw new ArgumentException();
            }

            var rabbitMqOptions = new RabbitMqMessageBrokerOptions();

            optionsAction.Invoke(rabbitMqOptions);

            brokerOptions.Services
                .AddSingleton<IRabbitMqMessageBrokerOptions>(rabbitMqOptions)
                .AddSingleton<IMessageBroker, RabbitMqMessageBroker>();
        }
    }
}
