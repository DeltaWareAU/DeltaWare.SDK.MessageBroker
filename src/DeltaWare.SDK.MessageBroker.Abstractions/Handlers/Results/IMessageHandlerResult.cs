﻿using System;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Handlers.Results
{
    public interface IMessageHandlerResult
    {
        public bool Retry { get; }

        public bool WasSuccessful { get; }

        public bool HasException { get; }

        public Exception? Exception { get; }

        public string? Message { get; }
    }
}
