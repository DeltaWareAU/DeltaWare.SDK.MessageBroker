using Microsoft.Extensions.DependencyInjection;

namespace DeltaWare.SDK.MessageBroker.Core.Options
{
    public sealed class MessageBrokerOptions
    {
        public IServiceCollection Services { get; }

        internal MessageBrokerOptions(IServiceCollection services)
        {
            Services = services;
        }
    }
}
