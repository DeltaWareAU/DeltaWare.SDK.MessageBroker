using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Messages.Properties
{
    public interface IPropertiesBuilder
    {
        IDictionary<string, object> BuildProperties<T>(T message) where T : class;
    }
}
