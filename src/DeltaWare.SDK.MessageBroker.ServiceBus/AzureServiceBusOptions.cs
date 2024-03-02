using DeltaWare.SDK.MessageBroker.Abstractions.Broker;
using DeltaWare.SDK.MessageBroker.Core.Options;
using DeltaWare.SDK.MessageBroker.ServiceBus.Broker;
using DeltaWare.SDK.MessageBroker.ServiceBus.Options;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            brokerOptions.Services
                .AddSingleton<IServiceBusMessageBrokerOptions>(new ServiceBusMessageBrokerOptions
                {
                    ConnectionString = connectionString
                })
                .AddSingleton<IMessageBroker, ServiceBusMessageBroker>();
        }
    }
}
