namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    public abstract class MessageGate : IDisposable
    {
        public bool IsOpen { get; protected set; }

        public bool IsClosed => !IsOpen;

        /// <summary>
        /// Waits for the Event Gate to be opened via an incoming message or the timeout has been breached.
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken"></param>
        public abstract Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default);

        /// <summary>
        /// Waits for the Event Gate to be opened via an incoming message or the timeout has been breached.
        /// </summary>
        /// <param name="cancellationToken"></param>
        public abstract Task WaitAsync(CancellationToken cancellationToken = default);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
