using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeltaWare.SDK.MessageBroker.Core.Handlers;

namespace DeltaWare.SDK.MessageBroker.Core.Binding.Helpers
{
    internal static class BindingHelper
    {
        public static IEnumerable<Type> GetProcessorTypesFromAssemblies(params Assembly[] assemblies)
            => assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsSubclassOfRawGeneric(typeof(MessageHandler<>))));

        public static IEnumerable<Type> GetMessageTypesFromAssemblies(params Assembly[] assemblies)
            => assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsClass));
    }
}
