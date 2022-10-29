using System.Collections.Generic;

namespace DeltaWare.SDK.MessageBroker.Core.Messages.Properties
{
    public interface IPropertiesProvider
    {
        IDictionary<string, object> GetProperties<T>(T message) where T : class;
    }
}
