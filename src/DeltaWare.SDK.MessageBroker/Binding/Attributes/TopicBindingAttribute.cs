using DeltaWare.SDK.Core.Validators;
using DeltaWare.SDK.MessageBroker.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Binding.Attributes
{
    public class TopicBindingAttribute : MessageBrokerBindingAttribute
    {
        public TopicBindingAttribute(string name, string routingPattern) : base(name, BrokerExchangeType.Topic, routingPattern)
        {
            StringValidator.ThrowOnNullOrWhitespace(routingPattern, nameof(routingPattern));
        }
    }
}
