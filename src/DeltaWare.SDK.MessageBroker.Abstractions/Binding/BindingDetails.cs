using System;
using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding
{
    public record BindingDetails
    {
        public string Name { get; }

        public string? RoutingPattern { get; }

        public BrokerExchangeType ExchangeType { get; }

        public BindingDetails(string name, string? routingPattern, BrokerExchangeType exchangeType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            RoutingPattern = routingPattern;
            ExchangeType = exchangeType;
        }
    }
}
