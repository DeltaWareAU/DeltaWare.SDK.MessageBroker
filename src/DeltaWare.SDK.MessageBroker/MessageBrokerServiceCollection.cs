using DeltaWare.SDK.MessageBroker.Abstractions.Binding;
using DeltaWare.SDK.MessageBroker.Abstractions.Broker;
using DeltaWare.SDK.MessageBroker.Abstractions.Handlers;
using DeltaWare.SDK.MessageBroker.Abstractions.Publisher;
using DeltaWare.SDK.MessageBroker.Core.Binding;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
using DeltaWare.SDK.MessageBroker.Core.Messages.Serialization;
using DeltaWare.SDK.MessageBroker.Core.Options;

/* Unmerged change from project 'DeltaWare.SDK.MessageBroker.Core (net7.0)'
Before:
using DeltaWare.SDK.MessageBroker.Abstractions.Publisher;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
After:
using DeltaWare.SDK.MessageBroker.Core.Publisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.MessageBroker.Core.Handlers;
*/

/* Unmerged change from project 'DeltaWare.SDK.MessageBroker.Core (net8.0)'
Before:
using DeltaWare.SDK.MessageBroker.Abstractions.Publisher;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
After:
using DeltaWare.SDK.MessageBroker.Core.Publisher;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.MessageBroker.Core.Handlers;
*/
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using DeltaWare.SDK.MessageBroker.Core.Broker.Hosting;

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
