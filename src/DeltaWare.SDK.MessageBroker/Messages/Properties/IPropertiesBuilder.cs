using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Core.Messages.Properties
{
    public interface IPropertiesBuilder
    {
        IDictionary<string, object> BuildProperties<T>(T message) where T : class;
    }
}
