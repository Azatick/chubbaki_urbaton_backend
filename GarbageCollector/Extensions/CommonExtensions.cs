using System.Collections.Generic;
using System.Linq;

namespace GarbageCollector.Extensions
{
    public static class CommonExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        public static bool IsNullOrEmpty(string @string)
        {
            return string.IsNullOrWhiteSpace(@string);
        }
    }
}