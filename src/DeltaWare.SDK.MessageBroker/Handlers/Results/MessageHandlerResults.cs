using System;
using System.Collections.Generic;
using System.Linq;
using DeltaWare.SDK.MessageBroker.Abstractions.Handlers.Results;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers.Results
{
    public class MessageHandlerResults : IMessageHandlerResults
    {
        public bool Retry => Results.Any(r => r is { Retry: true, WasSuccessful: false });
        public bool WasSuccessful => Results.All(r => r.WasSuccessful);
        public bool HasException => Results.Any(r => r.HasException);

        public IReadOnlyList<IMessageHandlerResult> Results { get; }

        public MessageHandlerResults(params IMessageHandlerResult[] results)
        {
            Results = new List<IMessageHandlerResult>(results);
        }

        public static IMessageHandlerResults Failure(Exception exception, string message)
        {
            return new MessageHandlerResults(MessageHandlerResult.Failure(exception, message));
        }

        public static IMessageHandlerResults Failure(string message)
        {
            return new MessageHandlerResults(MessageHandlerResult.Failure(message));
        }
    }
}
