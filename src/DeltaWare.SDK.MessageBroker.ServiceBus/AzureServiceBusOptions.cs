using DeltaWare.SDK.MessageBroker.Core.Broker;
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
