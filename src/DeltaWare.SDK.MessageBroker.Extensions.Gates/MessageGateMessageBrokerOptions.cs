﻿using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class MessageGateMessageBrokerOptions
    {
        public static void EnableMessageGates(this IMessageBrokerOptions options)
        {
            if (options is not MessageBrokerOptions brokerOptions)
            {
                throw new ArgumentException();
            }

            brokerOptions.Services
                .AddSingleton<MessageGateInterceptor>()
                .AddSingleton<IMessageInterceptor>(p => p.GetRequiredService<MessageGateInterceptor>())
                .AddSingleton<IMessageGateHandlerBinder>(p => p.GetRequiredService<MessageGateInterceptor>())
                .AddSingleton<IMessageGateProvider, MessageGateProvider>();
        }
    }
}
