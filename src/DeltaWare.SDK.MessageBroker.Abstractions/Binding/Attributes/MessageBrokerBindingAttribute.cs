using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;
using System;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MessageBrokerBindingAttribute : Attribute
    {
        public BindingDetails BindingDetails { get; }

        protected MessageBrokerBindingAttribute(string name, BrokerExchangeType exchangeType, string? routingPattern = null)
        {
            BindingDetails = new BindingDetails(name, routingPattern, exchangeType);
        }
    }
}
