using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.SDK.MessageBroker.Messages.Properties
{
    public class DefaultPropertyBuilder : IPropertiesBuilder
    {
        private readonly List<IPropertiesProvider> _providers = new();

        public DefaultPropertyBuilder(IServiceProvider serviceProvider)
        {
        }

        public IDictionary<string, object> BuildProperties<T>(T message) where T : class
            => _providers
                .SelectMany(provider => provider.GetProperties(message))
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
    }
}
