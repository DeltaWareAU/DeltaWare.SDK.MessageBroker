using DeltaWare.SDK.Core.Validators;
using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.ServiceBus.Broker;
using DeltaWare.SDK.MessageBroker.ServiceBus.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AzureServiceBusServiceCollection
    {
        public static IServiceCollection UseAzureServiceBus(this IServiceCollection serviceCollection, string connectionString)
        {
            StringValidator.ThrowOnNullOrWhitespace(connectionString, nameof(connectionString));

            serviceCollection
                .AddSingleton<IServiceBusMessageBrokerOptions>(new ServiceBusMessageBrokerOptions
                {
                    ConnectionString = connectionString
                })
                .UseMessageBroker()
                .AddSingleton<IMessageBroker, ServiceBusMessageBroker>();

            return serviceCollection;
        }
    }
}
