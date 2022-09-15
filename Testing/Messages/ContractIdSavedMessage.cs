using DeltaWare.SDK.MessageBroker.Binding.Attributes;
using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Processors;

namespace Testing.Messages
{
    [DirectBinding("contract.id.saved")]
    public class ContractIdSavedMessage : Message
    {
        public string ContractId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not ContractIdSavedMessage message)
            {
                return false;
            }

            return message.ContractId == ContractId;
        }
    }

    public class TempContractIdSavedHandler : MessageHandler<ContractIdSavedMessage>
    {
        protected override ValueTask ProcessAsync(ContractIdSavedMessage message)
        {
            return ValueTask.CompletedTask;
        }
    }
}
