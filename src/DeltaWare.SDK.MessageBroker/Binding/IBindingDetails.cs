using DeltaWare.SDK.MessageBroker.Core.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Core.Binding
{
    public interface IBindingDetails
    {
        string Name { get; }

        string? RoutingPattern { get; }

        BrokerExchangeType ExchangeType { get; }
    }
}
