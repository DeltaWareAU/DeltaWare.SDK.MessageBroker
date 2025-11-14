using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DeltaWare.SDK.MessageBroker.Core.Handlers.Results
{
    public class MessageHandlerResults
    {
        public bool Retry => Results.Any(r => r is { Retry: true, WasSuccessful: false });
        public bool WasSuccessful => Results.All(r => r.WasSuccessful);
        public bool HasException => Results.Any(r => r.HasException);

        public IReadOnlyList<MessageHandlerResult> Results { get; }

        public MessageHandlerResults(params MessageHandlerResult[] results)
        {
            Results = results.ToImmutableList();
        }

        public static MessageHandlerResults Failure(Exception exception, string message)
            => new(MessageHandlerResult.Failure(exception, message));

        public static MessageHandlerResults Failure(string message)
            => new(MessageHandlerResult.Failure(message));
    }
}
