using DeltaWare.SDK.MessageBroker.ServiceBus.Broker;
using DeltaWare.SDK.MessageBroker.ServiceBus.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.Options;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class AzureServiceBusOptions
    {
        public static void UseAzureServiceBus(this MessageBrokerOptions options, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            options.Services
                .AddSingleton(new ServiceBusMessageBrokerOptions { ConnectionString = connectionString })
                .AddSingleton<IMessageBroker, ServiceBusMessageBroker>();
        }
    }
}
