using DeltaWare.SDK.MessageBroker.Core.Messages.Interception;
using DeltaWare.SDK.MessageBroker.Core.Options;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;
using Microsoft.Extensions.DependencyInjection;
using System;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class MessageGateOptions
    {
        public static void EnableMessageGates(this MessageBrokerOptions options)
        {
            options.Services
                .AddSingleton<MessageGateInterceptor>()
                .AddSingleton<MessageInterceptor>(p => p.GetRequiredService<MessageGateInterceptor>())
                .AddSingleton<IMessageGateHandlerBinder>(p => p.GetRequiredService<MessageGateInterceptor>())
                .AddSingleton<IMessageGateProvider, MessageGateProvider>();
        }
    }
}
