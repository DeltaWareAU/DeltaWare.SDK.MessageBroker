using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DeltaWare.SDK.MessageBroker.Messages;
using DeltaWare.SDK.MessageBroker.Processors;

namespace DeltaWare.SDK.MessageBroker.Binding.Helpers
{
    internal static class BindingHelper
    {
        public static IEnumerable<Type> GetProcessorTypesFromAssemblies(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsSubclassOfRawGeneric(typeof(MessageHandler<>))));

        }

        public static IEnumerable<Type> GetMessageTypesFromAssemblies(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(a => a.GetLoadedTypes().Where(t => t.IsSubclassOf<Message>()));

        }
    }
}
