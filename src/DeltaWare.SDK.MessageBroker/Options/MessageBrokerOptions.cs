using Microsoft.Extensions.DependencyInjection;

namespace DeltaWare.SDK.MessageBroker.Core.Options
{
    public class MessageBrokerOptions : IMessageBrokerOptions
    {
        public IServiceCollection Services { get; }

        internal MessageBrokerOptions(IServiceCollection services)
        {
            Services = services;
        }
    }
}
