using DeltaWare.SDK.MessageBroker.Core.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Core.Binding.Attributes
{
    public class DirectBindingAttribute : MessageBrokerBindingAttribute
    {
        public DirectBindingAttribute(string name) : base(name, BrokerExchangeType.Direct)
        {
        }
    }
}
