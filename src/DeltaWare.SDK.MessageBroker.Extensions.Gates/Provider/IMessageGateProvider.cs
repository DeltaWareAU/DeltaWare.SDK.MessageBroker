namespace DeltaWare.SDK.MessageBroker.Extensions.Gates.Provider
{
    public interface IMessageGateProvider
    {
        /// <summary>
        /// Initiates a new Gate that can only be Opened by a Message Matching the Key Specified.
        /// </summary>
        /// <typeparam name="TKey">The Message Type.</typeparam>
        /// <param name="key">The Message required for the <see cref="MessageGate"/> to Open.</param>
        /// <returns>Returns a new Instance of a <see cref="MessageGate"/> that can Awaited.</returns>
        /// <remarks>The Equals Method is used </remarks>
        MessageGate InitiateGate<TKey>(TKey key) where TKey : class;
    }
}
