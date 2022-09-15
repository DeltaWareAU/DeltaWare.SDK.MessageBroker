using DeltaWare.SDK.MessageBroker.Binding.Attributes;
using DeltaWare.SDK.MessageBroker.Messages;

namespace Testing.Messages
{
    [DirectBinding("Contract.Id.Saved")]
    public class ContractIdSavedMessage : Message
    {
        public string ContractId { get; set; }
    }
}
