using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsNullOrEmpty<T, U>(this IDictionary<T, U> dictionary)
        {
            return dictionary == null || dictionary.Keys.Count == 0;
        }
    }
}
