using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Broker;
using DeltaWare.SDK.MessageBroker.RabbitMQ.Options;
using System;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class AzureServiceBusServiceCollection
    {
        public static IServiceCollection UseRabbitMQ(this IServiceCollection serviceCollection, Action<RabbitMqMessageBrokerOptions> optionsAction)
        {
            var options = new RabbitMqMessageBrokerOptions();

            optionsAction.Invoke(options);

            serviceCollection
                .UseMessageBroker()
                .AddSingleton<IRabbitMqMessageBrokerOptions>(options)
                .AddSingleton<IMessageBroker, RabbitMqMessageBroker>();

            return serviceCollection;
        }
    }
}
