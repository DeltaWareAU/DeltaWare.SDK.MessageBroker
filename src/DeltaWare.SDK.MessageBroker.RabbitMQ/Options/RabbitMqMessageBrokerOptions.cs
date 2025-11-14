namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Options
{


    public sealed record RabbitMqMessageBrokerOptions
    {
        public string UserName { get; init; }
        public string Password { get; init; }
        public string HostName { get; init; }
        public string VirtualHost { get; init; }
        public int Port { get; init; }
    }
}
