using DeltaWare.SDK.MessageBroker.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Binding.Attributes
{
    public class DirectBindingAttribute : MessageBrokerBindingAttribute
    {
        public DirectBindingAttribute(string name) : base(name, BrokerExchangeType.Direct)
        {
        }
    }
}
