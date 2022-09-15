﻿using Microsoft.Extensions.DependencyInjection;

namespace DeltaWare.SDK.MessageBroker
{
    public class MessageBrokerOptions : IMessageBrokerOptions
    {
        public IServiceCollection Services { get; }

        internal MessageBrokerOptions(IServiceCollection services)
        {
            Services = services;
        }
    }

    public interface IMessageBrokerOptions
    {
    }
}