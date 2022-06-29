using DeltaWare.SDK.MessageBroker.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Binding.Attributes
{
    public class FanoutBindingAttribute : MessageBrokerBindingAttribute
    {
        public FanoutBindingAttribute(string name) : base(name, BrokerExchangeType.Fanout)
        {
        }
    }
}
