using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes;

namespace Testing.Messages
{
    [DirectBinding("contract.create")]
    public class CreateContractMessage
    {
        public string ContractId { get; set; }

        public string ApplicationId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
