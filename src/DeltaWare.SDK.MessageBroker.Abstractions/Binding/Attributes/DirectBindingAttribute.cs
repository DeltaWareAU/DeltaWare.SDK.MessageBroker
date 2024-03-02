using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes
{
    public class DirectBindingAttribute : MessageBrokerBindingAttribute
    {
        public DirectBindingAttribute(string name) : base(name, BrokerExchangeType.Direct)
        {
        }
    }
}
