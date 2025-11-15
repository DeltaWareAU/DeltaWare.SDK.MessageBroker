using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeltaWare.SDK.MessageBroker.Handlers;
using AssemblyExtensions = System.AssemblyExtensions;
using TypeExtensions = System.TypeExtensions;

namespace DeltaWare.SDK.MessageBroker.Binding.Helpers
{
    internal static class BindingHelper
    {
        public static IEnumerable<Type> GetProcessorTypesFromAssemblies(params Assembly[] assemblies)
            => assemblies.SelectMany(a => AssemblyExtensions.GetLoadedTypes(a).Where(t => TypeExtensions.IsSubclassOfRawGeneric(t, typeof(MessageHandler<>))));

        public static IEnumerable<Type> GetMessageTypesFromAssemblies(params Assembly[] assemblies)
            => assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsClass));
    }
}
