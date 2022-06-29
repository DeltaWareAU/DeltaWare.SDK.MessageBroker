using System;
using System.Text.Json;

namespace DeltaWare.SDK.MessageBroker.Messages.Serialization
{
    public interface IMessageSerializer
    {
        Message Deserialize(string message, Type type);

        string Serialize<TMessage>(TMessage message) where TMessage : Message;
    }

    internal class DefaultMessageSerializer : IMessageSerializer
    {
        public Message Deserialize(string message, Type type)
        {
            return (Message)JsonSerializer.Deserialize(message, type);
        }

        public string Serialize<TMessage>(TMessage message) where TMessage : Message
        {
            return JsonSerializer.Serialize(message);
        }
    }
}
