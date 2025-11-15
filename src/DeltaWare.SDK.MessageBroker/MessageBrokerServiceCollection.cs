using DeltaWare.SDK.MessageBroker.Abstractions.Publisher;
using DeltaWare.SDK.MessageBroker.Binding;
using DeltaWare.SDK.MessageBroker.Broker;
using DeltaWare.SDK.MessageBroker.Broker.Hosting;
using DeltaWare.SDK.MessageBroker.Handlers;
using DeltaWare.SDK.MessageBroker.Messages.Serialization;
using DeltaWare.SDK.MessageBroker.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class MessageBrokerServiceCollection
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, Action<MessageBrokerOptions> optionsBuilder)
        {
            var options = new MessageBrokerOptions(services);

            optionsBuilder.Invoke(options);

            services.TryAddSingleton<IMessageSerializer, DefaultMessageSerializer>();
            services.TryAddSingleton<IMessageHandlerManager, MessageHandlerManager>();
            services.TryAddSingleton<IMessagePublisher>(p => p.GetRequiredService<IMessageBroker>());
            services.TryAddSingleton<IBindingDirector, BindingDirector>();
            services.AddHostedService<MessageBrokerHost>();

            return services;
        }
    }
}
