namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    /// <summary>
    /// Represents a Gate that can be Opened by an Incoming Message.
    /// </summary>
    public abstract class MessageGate : IDisposable
    {
        /// <summary>
        /// Specifies if the Gate is Open.
        /// </summary>
        public bool IsOpen { get; protected set; }

        /// <summary>
        /// Waits for the Gate to be Opened via an Incoming Message or the Timeout has Expired.
        /// </summary>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> token to observe.</param>
        public abstract Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default);

        /// <summary>
        /// Waits for the Gate to be Opened via an Incoming Message or the Timeout has Expired.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> token to observe.</param>
        public abstract Task WaitAsync(CancellationToken cancellationToken = default);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
