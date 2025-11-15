using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeltaWare.SDK.MessageBroker.Broker.Hosting
{
    internal sealed class MessageBrokerHost : IHostedService
    {
        private readonly IMessageBroker _messageBroker;

        private readonly ILogger _logger;

        public MessageBrokerHost(ILogger<MessageBrokerHost> logger, IMessageBroker messageBroker)
        {
            _logger = logger;
            _messageBroker = messageBroker;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Message Broker Host.");

            if (!_messageBroker.Initiated)
            {
                _logger.LogDebug("Initiating Message Consumer Bindings");

                await _messageBroker.InitiateBindingsAsync(cancellationToken);
            }

            await _messageBroker.StartListeningAsync(cancellationToken);

            _logger.LogInformation("Message Broker Host has Started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Message Broker Host.");

            await _messageBroker.StopListeningAsync(cancellationToken);

            while (_messageBroker.IsProcessing && !cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Shutdown postponed until messages have finished being processed.");

                await Task.Delay(1000, cancellationToken);
            }

            _logger.LogInformation("Message Broker Host Stopped.");
        }
    }
}
