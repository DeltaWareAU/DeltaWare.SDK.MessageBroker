namespace DeltaWare.SDK.MessageBroker.ServiceBus.Options
{
    public interface IServiceBusMessageBrokerOptions
    {
        string ConnectionString { get; }
    }

    public class ServiceBusMessageBrokerOptions : IServiceBusMessageBrokerOptions
    {
        public string ConnectionString { get; set; }
    }
}
