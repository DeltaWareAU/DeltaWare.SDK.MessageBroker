using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding
{
    public sealed class BindingDetails : IBindingDetails
    {
        public string Name { get; init; } = null!;

        public string? RoutingPattern { get; init; }

        public BrokerExchangeType ExchangeType { get; init; }

    }
}
