namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    public abstract class EventGate : IDisposable
    {
        public bool IsOpen { get; protected set; }

        public bool IsClosed => !IsOpen;

        public abstract Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default);

        public abstract Task WaitAsync(CancellationToken cancellationToken = default);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);
    }
}
