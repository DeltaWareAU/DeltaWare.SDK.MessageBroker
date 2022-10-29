using DeltaWare.SDK.MessageBroker.Core.Broker.Hosting;
using DeltaWare.SDK.MessageBroker.Core.Handlers;
using DeltaWare.SDK.MessageBroker.Core.Publisher;
using System.Threading;
using System.Threading.Tasks;

namespace DeltaWare.SDK.MessageBroker.Core.Broker
{
    public interface IMessageBroker : IMessagePublisher
    {
        /// <summary>
        /// Indicates if the <see cref="IMessageBroker"/> is Initiated.
        /// </summary>
        bool Initiated { get; }

        /// <summary>
        /// Indicates if the <see cref="IMessageBroker"/> is Actively Listening for Messages.
        /// </summary>
        bool IsListening { get; }

        /// <summary>
        /// Indicates if the <see cref="IMessageBroker"/> is Actively Processing Messages.
        /// </summary>
        bool IsProcessing { get; }

        /// <summary>
        /// Binds the <see cref="MessageHandler{TMessage}"/> to the <see cref="IMessageBroker"/>.
        /// </summary>
        /// <remarks>This is called by the <see cref="MessageBrokerHost"/> before the <see cref="StartListeningAsync"/> Method.</remarks>
        void InitiateBindings();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StartListeningAsync(CancellationToken cancellationToken = default);

        Task StopListeningAsync(CancellationToken cancellationToken = default);
    }
}
