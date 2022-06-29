using System;
using System.Collections.Generic;
using System.Linq;

namespace DeltaWare.SDK.MessageBroker.Processors.Results
{
    public interface IMessageHandlerResults
    {
        IReadOnlyList<IMessageHandlerResult> Results { get; }

        public bool Retry { get; }

        public bool WasSuccessful { get; }

        public bool HasException { get; }
    }

    public class MessageHandlerResults : IMessageHandlerResults
    {
        public bool Retry => Results.Any(r => r.Retry && !r.WasSuccessful);
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
    }
}
