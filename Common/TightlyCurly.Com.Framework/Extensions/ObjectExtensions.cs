using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TightlyCurly.Com.Framework.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object value)
        {
            if (value == null)
            {
                return new Dictionary<string, object>();
            }

            var valueType = value.GetType();
            var properties = valueType.GetProperties(BindingFlags.Public);

            return properties
                .ToDictionary(property => property.Name, property =>
                    property.GetValue(value, null));
        }

        public static TValue DoIfDefault<TValue>(this TValue value, Func<TValue> doDelegate)
        {
            if (value.IsNull())
            {
                return doDelegate();
            }

            return value;
        }

        public static TValue Do<TValue>(this TValue value, Func<TValue> doDelegate)
        {
            if (value.IsNotNull())
            {
                return doDelegate();
            }

            return value;
        }

        public static TReturn Do<TValue, TReturn>(this TValue value, Func<TReturn> doDelegate)
        {
            if (value.IsNotNull())
            {
                return doDelegate();
            }

            return default(TReturn);
        }

        public static TValue Do<TValue>(this TValue value, Action doDelegate)
        {
            if (value.IsNotNull())
            {
                doDelegate();
            }

            return value;
        }

        public static IEnumerable<IDictionary<string, object>> ToDictionaries(IEnumerable<object> values)
        {
            if (values.IsNullOrEmpty())
            {
                return new List<IDictionary<string, object>>();
            }

            return values
                .Select(value => value.ToDictionary())
                .ToList();
        }

        public static bool IsNull(this object value)
        {
            return value == null;
        }

        public static bool IsNotNull(this object value)
        {
            return value != null;
        }

        public static bool SafeEquals(this object value, object other)
        {
            if (ReferenceEquals(value, other))
            {
                return true;
            }

            if (value.IsNull() && other.IsNull())
            {
                return false;
            }

            if (value.IsNull() && other.IsNotNull())
            {
                return false;
            }

            if (value.IsNotNull() && other.IsNull())
            {
                return false;
            }

            return value.Equals(other);
        }
    }
}
