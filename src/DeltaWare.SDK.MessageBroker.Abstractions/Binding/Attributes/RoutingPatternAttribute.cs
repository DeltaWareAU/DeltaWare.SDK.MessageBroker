using System;

namespace DeltaWare.SDK.MessageBroker.Abstractions.Binding.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutingPatternAttribute : Attribute
    {
        public string Pattern { get; }

        public RoutingPatternAttribute(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            Pattern = pattern;
        }
    }
}
