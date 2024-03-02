using System.Linq;
using System.Reflection;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class ServiceProviderExtensions
    {
        public static object CreateInstance(this IServiceProvider provider, Type type)
        {
            ConstructorInfo[] constructors = type.GetConstructors();

            if (constructors.Length > 1)
            {
                throw new ArgumentException($"Only one construct may be present for the given type {type.Name}.", nameof(type));
            }

            ConstructorInfo constructor = constructors.First();

            ParameterInfo[] parameters = constructor.GetParameters();

            object[] arguments = new object[parameters.Length];

            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = provider.GetService(parameters[i].ParameterType)!;
            }

            return Activator.CreateInstance(type, arguments)!;
        }
    }
}
