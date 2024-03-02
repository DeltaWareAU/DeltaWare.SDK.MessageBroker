// ReSharper disable once CheckNamespace
using System.Reflection;

namespace System
{
    internal static class TypeExtensions
    {
        public static Type[] GetGenericArguments(this Type type, Type genericType)
        {
            if (!genericType.IsGenericType)
            {
                throw new ArgumentException();
            }

            while (type!.BaseType != null || type == typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    return type.GetGenericArguments();
                }

                type = type!.BaseType!;
            }

            return Array.Empty<Type>();
        }

        public static bool TryGetCustomAttribute<T>(this Type type, out T? attribute) where T : Attribute
        {
            attribute = type.GetCustomAttribute<T>();

            return attribute != null;
        }

        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType)
        {
            if (!genericType.IsGenericType)
            {
                throw new ArgumentException();
            }

            if (type == genericType)
            {
                return false;
            }

            while (type.BaseType != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }

                if (type == genericType)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }
}
