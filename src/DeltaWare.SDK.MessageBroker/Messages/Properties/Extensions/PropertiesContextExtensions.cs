// ReSharper disable once CheckNamespace
namespace DeltaWare.SDK.MessageBroker.Core.Messages.Properties
{
    public static class PropertiesContextExtensions
    {
        public static T? GetPropertyValue<T>(this IPropertiesContext context, string key) where T : class
            => (T?)context[key];

        public static bool TryGetPropertyValue<T>(this IPropertiesContext context, string key, out T? value)
            where T : class
        {
            value = (T?)context[key];

            return value != null;
        }

        public static bool Contains(this IPropertiesContext context, string key)
            => context[key] != null;
    }
}
