﻿namespace DeltaWare.SDK.MessageBroker.RabbitMQ.Options
{


    public class RabbitMqMessageBrokerOptions : IRabbitMqMessageBrokerOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public int Port { get; set; }
    }
}
