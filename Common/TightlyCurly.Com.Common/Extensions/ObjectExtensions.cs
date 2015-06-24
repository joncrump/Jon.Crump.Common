using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object value, 
            IEnumerable<Type> selectorAttributeTypes = null, string format = null)
        {
            if (value == null)
            {
                return new Dictionary<string, object>();
            }

            if (selectorAttributeTypes.IsNullOrEmpty())
            {
                return ToDictionaryInternal(value);
            }

            var valueType = value.GetType();
            var properties = valueType.GetProperties();
            var tempValues = new List<Tuple<string, object, int>>();

            foreach (var property in properties)
            {
                var added = false;
                var attributes = property.GetCustomAttributes()
                    .ToSafeList();

                if (attributes.Where(t => t.GetType() == typeof (IgnoreAttribute)).IsNotNullOrEmpty())
                {
                    continue;
                }
                
                foreach (var tempAttribute in attributes)
                {
                    foreach (var selectorAttributeType in selectorAttributeTypes)
                    {
                        added = AddPropertyValueInternal(selectorAttributeType, tempAttribute, tempValues, property, value, format);

                        if (added)
                        {
                            break;
                        }
                    }

                    if (added)
                    {
                        break;
                    }
                }
            }

            return tempValues
                .OrderBy(t => t.Item3)
                .ToDictionary(t => t.Item1, t => t.Item2);
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

            return default (TReturn);
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

        private static bool AddPropertyValueInternal(Type selectorAttributeType, 
            Attribute tempAttribute, List<Tuple<string, object, int>> values,
            PropertyInfo property, object instance, string format = null)
        {
            var added = false;

            if (tempAttribute.GetType() == selectorAttributeType)
            {
                var orderable = tempAttribute as IOrderable;

                if (orderable.IsNotNull())
                {
                    values.Add(new Tuple<string, object, int>(
                        property.Name, GetValue(property, instance, format), orderable.Order.HasValue ? orderable.Order.Value : 0));
                }
                else
                {
                    values.Add(new Tuple<string, object, int>(
                        property.Name, GetValue(property, instance, format), 0));
                }

                added = true;
            }

            return added;
        }

        private static object GetValue(PropertyInfo property, object instance, string format = null)
        {
            var value = property.GetValue(instance, null);

            if (property.PropertyType == typeof(string) && format.IsNotNullOrEmpty())
            {
                value = format.FormatString(value);
            }

            return value;
        }

        private static IDictionary<string, object> ToDictionaryInternal(this object value)
        {
            if (value == null)
            {
                return new Dictionary<string, object>();
            }

            var valueType = value.GetType();
            var properties = valueType.GetProperties();

            return properties.ToDictionary(property => property.Name,
                property => property.GetValue(value));
        }
    }
}
