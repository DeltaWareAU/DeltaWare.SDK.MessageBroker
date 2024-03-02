using DeltaWare.SDK.MessageBroker.Abstractions.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding
{
    public interface IBindingDetails
    {
        string Name { get; }

        string? RoutingPattern { get; }

        BrokerExchangeType ExchangeType { get; }
    }
}
