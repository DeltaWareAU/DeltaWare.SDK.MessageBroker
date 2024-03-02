using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Handlers.Results
{
    public interface IMessageHandlerResults
    {
        IReadOnlyList<IMessageHandlerResult> Results { get; }

        public bool Retry { get; }

        public bool WasSuccessful { get; }

        public bool HasException { get; }
    }
}
