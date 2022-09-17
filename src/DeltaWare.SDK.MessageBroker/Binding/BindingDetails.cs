using DeltaWare.SDK.MessageBroker.Core.Binding.Enums;

namespace DeltaWare.SDK.MessageBroker.Core.Binding
{
    public class BindingDetails : IBindingDetails
    {
        public string Name { get; init; }
        public string? RoutingPattern { get; init; }
        public BrokerExchangeType ExchangeType { get; init; }

    }
}
