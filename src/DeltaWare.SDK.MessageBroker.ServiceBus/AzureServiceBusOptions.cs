using DeltaWare.SDK.Core.Validators;
using DeltaWare.SDK.MessageBroker.ServiceBus.Broker;
using DeltaWare.SDK.MessageBroker.ServiceBus.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using DeltaWare.SDK.MessageBroker.Core.Broker;
using DeltaWare.SDK.MessageBroker.Core.Options;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class AzureServiceBusOptions
    {
        public static void UseAzureServiceBus(this IMessageBrokerOptions options, string connectionString)
        {
            if (options is not MessageBrokerOptions brokerOptions)
            {
                throw new ArgumentException();
            }

            StringValidator.ThrowOnNullOrWhitespace(connectionString, nameof(connectionString));

            brokerOptions.Services
                .AddSingleton<IServiceBusMessageBrokerOptions>(new ServiceBusMessageBrokerOptions
                {
                    ConnectionString = connectionString
                })
                .AddSingleton<IMessageBroker, ServiceBusMessageBroker>();
        }
    }
}
