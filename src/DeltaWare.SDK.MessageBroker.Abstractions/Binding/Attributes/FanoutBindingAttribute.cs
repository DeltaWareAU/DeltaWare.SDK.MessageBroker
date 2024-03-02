using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes
{
    public class FanoutBindingAttribute : MessageBrokerBindingAttribute
    {
        public FanoutBindingAttribute(string name) : base(name, BrokerExchangeType.Fanout)
        {
        }
    }
}
