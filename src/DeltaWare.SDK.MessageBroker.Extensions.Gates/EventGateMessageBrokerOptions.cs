using DeltaWare.SDK.MessageBroker.Extensions.Gates.Interceptor;
using DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker
{
    public static class EventGateMessageBrokerOptions
    {
        public static void EnableEventGates(this IMessageBrokerOptions options)
        {
            if (options is not MessageBrokerOptions brokerOptions)
            {
                throw new ArgumentException();
            }

            brokerOptions.Services.AddScoped<EventGateMessageInterceptor>();
            brokerOptions.Services.AddSingleton<IEventGateProvider, EventGateProvider>();
            brokerOptions.Services.AddSingleton<IMessageInterceptor>(p => p.GetRequiredService<EventGateMessageInterceptor>());
            brokerOptions.Services.AddSingleton<IEventGateHandlerBinder>(p => p.GetRequiredService<EventGateMessageInterceptor>());
        }
    }
}
