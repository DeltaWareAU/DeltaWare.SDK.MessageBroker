using System;
using System.Text.Json.Serialization;

namespace DeltaWare.SDK.MessageBroker.Messages
{
    public abstract class Message
    {
        protected Message()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        protected Message(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonInclude]
        public Guid Id { get; set; }

        [JsonInclude]
        public DateTime CreationDate { get; set; }
    }
}
