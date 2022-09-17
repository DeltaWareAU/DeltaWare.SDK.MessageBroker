using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
    internal static class LinqExtensions
    {
        public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            foreach (TSource item in source)
            {
                action.Invoke(item);
            }
        }
    }
}
