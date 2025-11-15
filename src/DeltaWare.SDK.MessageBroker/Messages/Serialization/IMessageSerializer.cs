using System;

namespace DeltaWare.SDK.MessageBroker.Messages.Serialization
{
    public interface IMessageSerializer
    {
        object? Deserialize(string message, Type type);

        string Serialize<TMessage>(TMessage message) where TMessage : class;
    }
}
