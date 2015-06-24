using System;
using System.Reflection;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static MethodInfo LocateMethod(this object value, string name)
        {
            if (value.IsNull() || name.IsNullOrEmpty())
            {
                return null;
            }

            var method = value.GetType().GetMethod(name);

            return method;
        }

        public static MethodInfo Locate(this object value, string name, Type[] types)
        {
            if (value.IsNull() || name.IsNullOrEmpty())
            {
                return null;
            }

            var method = value.GetType().GetMethod(name, types);

            return method;
        }
    }
}
