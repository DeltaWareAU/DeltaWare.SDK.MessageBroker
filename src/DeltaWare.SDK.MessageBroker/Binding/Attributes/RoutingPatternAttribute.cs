using DeltaWare.SDK.Core.Validators;
using System;

namespace DeltaWare.SDK.MessageBroker.Core.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutingPatternAttribute : Attribute
    {
        public string Pattern { get; }

        public RoutingPatternAttribute(string pattern)
        {
            StringValidator.ThrowOnNullOrWhitespace(pattern, nameof(pattern));

            Pattern = pattern;
        }
    }
}
