﻿using DeltaWare.SDK.MessageBroker.Core.Messages;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider
{
    public interface IMessageGateProvider
    {
        MessageGate InitiateGate<TKey>(TKey key) where TKey : Message;
    }
}
