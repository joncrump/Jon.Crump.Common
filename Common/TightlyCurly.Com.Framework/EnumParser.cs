using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class EnumParser
    {
        public static T Parse<T>(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
