using System;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes
{
    public class TopicBindingAttribute : MessageBrokerBindingAttribute
    {
        public TopicBindingAttribute(string name, string routingPattern) : base(name, BrokerExchangeType.Topic, routingPattern)
        {
            if (string.IsNullOrWhiteSpace(routingPattern))
            {
                throw new ArgumentNullException(nameof(routingPattern));
            }
        }
    }
}
