using System;
using System.Text.Json;

namespace DeltaWare.SDK.MessageBroker.Core.Messages.Serialization
{
    internal sealed class DefaultMessageSerializer : IMessageSerializer
    {
        public object? Deserialize(string message, Type type)
        {
            return JsonSerializer.Deserialize(message, type);
        }

        public string Serialize<TMessage>(TMessage message) where TMessage : class
        {
            return JsonSerializer.Serialize(message);
        }
    }
}
