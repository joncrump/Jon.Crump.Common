using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TightlyCurly.Com.Framework.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool IsNullOrEmpty<T, U>(this IDictionary<T, U> dictionary)
        {
            return dictionary == null || dictionary.Keys.Count == 0;
        }
    }
}
