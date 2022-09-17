namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Options
{
    public interface IRabbitMqMessageBrokerOptions
    {
        string UserName { get; }

        string Password { get; }

        string HostName { get; }

        string VirtualHost { get; }

        int Port { get; }
    }
}
