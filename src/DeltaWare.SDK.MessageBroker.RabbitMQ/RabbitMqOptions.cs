using DeltaWare.SDK.MessageBroker.Core.Broker;
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
