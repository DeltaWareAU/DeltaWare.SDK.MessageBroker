using System;

namespace DeltaWare.SDK.MessageBroker.Handlers.Results
{
    public record MessageHandlerResult
    {
        public bool WasSuccessful { get; }
        public bool Retry { get; }
        public bool HasException { get; }
        public Exception? Exception { get; }
        public string? Message { get; }

        public MessageHandlerResult(bool wasSuccessful, bool retry = false, Exception? exception = null, string? message = null)
        {
            WasSuccessful = wasSuccessful;
            Retry = retry;
            HasException = exception != null;
            Exception = exception;
            Message = message;
        }

        public static MessageHandlerResult Success()
            => new(true);

        public static MessageHandlerResult Failure(string message, bool retry = false)
            => new(false, retry, null, message);

        public static MessageHandlerResult Failure(Exception ex, bool retry = false)
            => new(false, retry, ex, ex.Message);

        public static MessageHandlerResult Failure(Exception ex, string message, bool retry = false)
            => new(false, retry, ex, message);
    }
}
