namespace DeltaWare.SDK.MessageBroker.Messages.Properties
{
    public interface IPropertiesContext
    {
        object? this[string key] { get; }
    }
}
