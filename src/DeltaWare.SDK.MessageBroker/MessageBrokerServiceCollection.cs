using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Broker;
using DeltaWare.SDK.MessageBroker.Core.Broker.Hosting;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
using DeltaWare.SDK.MessageBroker.Core.Messages.Serialization;
using DeltaWare.SDK.MessageBroker.Core.Options;
using DeltaWare.SDK.MessageBroker.Core.Publisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class MessageBrokerServiceCollection
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, Action<IMessageBrokerOptions> optionsBuilder)
        {
            MessageBrokerOptions options = new MessageBrokerOptions(services);

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
