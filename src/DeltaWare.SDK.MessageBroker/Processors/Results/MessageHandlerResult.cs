using System;

namespace DeltaWare.SDK.MessageBroker.Processors.Results
{
    public class MessageHandlerResult : IMessageHandlerResult
    {
        public bool Retry { get; init; }

        public bool WasSuccessful { get; init; }

        public bool HasException => Exception != null;

        public Exception? Exception { get; init; }

        public string? Message { get; init; }

        public static IMessageHandlerResult Success() => new MessageHandlerResult
        {
            WasSuccessful = true
        };

        public static IMessageHandlerResult Failure(string message, bool retry = false) => new MessageHandlerResult
        {
            Message = message,
            Retry = retry,
            WasSuccessful = false
        };

        public static IMessageHandlerResult Failure(Exception exception, bool retry = false) => new MessageHandlerResult
        {
            Exception = exception,
            Message = exception.Message,
            WasSuccessful = false,
            Retry = retry
        };

        public static IMessageHandlerResult Failure(Exception exception, string message, bool retry = false) => new MessageHandlerResult
        {
            Exception = exception,
            Message = message,
            WasSuccessful = false,
            Retry = retry
        };
    }
}
