using DeltaWare.SDK.MessageBroker.Core.Binding.Attributes;
using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace Testing.Messages
{
    [DirectBinding("contract.create")]
    public class CreateContractMessage : Message
    {
        public string ContractId { get; set; }

        public string ApplicationId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
