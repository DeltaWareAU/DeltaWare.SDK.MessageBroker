using DeltaWare.SDK.MessageBroker.Core.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Core.Binding.Attributes
{
    public class FanoutBindingAttribute : MessageBrokerBindingAttribute
    {
        public FanoutBindingAttribute(string name) : base(name, BrokerExchangeType.Fanout)
        {
        }
    }
}
