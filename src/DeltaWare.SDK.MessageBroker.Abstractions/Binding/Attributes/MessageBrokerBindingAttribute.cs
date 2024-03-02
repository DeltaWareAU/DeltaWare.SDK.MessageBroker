using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;
using System;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MessageBrokerBindingAttribute : Attribute
    {
        public string Name { get; }

        public BrokerExchangeType ExchangeType { get; }

        public string? RoutingPattern { get; }

        protected MessageBrokerBindingAttribute(string name, BrokerExchangeType exchangeType, string? routingPattern = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            ExchangeType = exchangeType;
            RoutingPattern = routingPattern;
        }

        public IBindingDetails GetBindingDetails()
        {
            return new BindingDetails
            {
                Name = Name,
                RoutingPattern = RoutingPattern,
                ExchangeType = ExchangeType
            };
        }
    }
}
