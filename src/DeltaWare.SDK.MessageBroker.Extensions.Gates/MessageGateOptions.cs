using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;
using DeltaWare.SDK.MessageBroker.Messages.Interception;
using DeltaWare.SDK.MessageBroker.Options;
using Microsoft.Extensions.DependencyInjection;

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
