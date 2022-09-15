using DeltaWare.SDK.Core.Validators;
using DeltaWare.SDK.MessageBroker.Binding.Enums;
using System;

namespace DeltaWare.SDK.MessageBroker.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MessageBrokerBindingAttribute : Attribute
    {
        public string Name { get; }

        public BrokerExchangeType ExchangeType { get; }

        public string? RoutingPattern { get; }

        protected MessageBrokerBindingAttribute(string name, BrokerExchangeType exchangeType, string? routingPattern = null)
        {
            StringValidator.ThrowOnNullOrWhitespace(name, nameof(name));

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
