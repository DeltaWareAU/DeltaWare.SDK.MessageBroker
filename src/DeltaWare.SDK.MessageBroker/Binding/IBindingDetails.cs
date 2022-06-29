using DeltaWare.SDK.MessageBroker.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Binding
{
    public interface IBindingDetails
    {
        string Name { get; }

        string? RoutingPattern { get; }

        BrokerExchangeType ExchangeType { get; }
    }
}
