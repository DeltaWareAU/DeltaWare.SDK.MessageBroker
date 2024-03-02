using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Extensions.Gates
{
    internal abstract class MessageGate<TKey> : MessageGate where TKey : class
    {
        private readonly TaskCompletionSource _isUnlocked = new();

        private readonly TKey _key;

        private readonly TimeSpan _timeout;

        protected MessageGate(TKey key, TimeSpan timeout)
        {
            _key = key;
            _timeout = timeout;
        }

        protected bool TryOpen(TKey key)
        {
            if (IsOpen || !_key.Equals(key))
            {
                return false;
            }

            _isUnlocked.SetResult();

            return true;
        }

        public override Task WaitAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            return WaitUntilCountOrTimeoutAsync((int)timeout.TotalMilliseconds, cancellationToken);
        }

        public override Task WaitAsync(CancellationToken cancellationToken = default)
        {
            return WaitUntilCountOrTimeoutAsync((int)_timeout.TotalMilliseconds, cancellationToken);
        }

        private async Task WaitUntilCountOrTimeoutAsync(int millisecondsTimeout, CancellationToken cancellationToken)
        {
            if (millisecondsTimeout is Timeout.Infinite or <= 0)
            {
                throw new ArgumentException("Timeout cannot be 0 or infinite.");
            }

            if (IsOpen)
            {
                return;
            }

            using CancellationTokenSource cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            await Task.WhenAny(_isUnlocked.Task, Task.Delay(millisecondsTimeout, cancellationSource.Token));

            IsOpen = true;
        }
    }
}