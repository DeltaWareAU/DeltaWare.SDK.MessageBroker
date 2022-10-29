namespace DeltaWare.SDK.MessageBroker.Core.Messages.Properties
{
    public interface IPropertiesContext
    {
        object? this[string key] { get; }
    }
}
