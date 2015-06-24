using System;
using System.Collections.Generic;
using System.Linq;

namespace TightlyCurly.Com.Common.Models
{
    public class PropertyBag
    {
        private readonly IDictionary<string, Tuple<Type, object>> _values;

        public PropertyBag()
        {
            _values = new Dictionary<string, Tuple<Type, object>>();
        }

        public void Add(string key, Type type, object value)
        {
            Guard.EnsureIsNotNullOrEmpty("key", key);

            _values.Add(key, new Tuple<Type, object>(type, value));
        }

        public TValue Get<TValue>()
        {
            var value = _values.Values
                .FirstOrDefault(v => v.Item1 == typeof(TValue));

            if (value != null)
            {
                return (TValue)value.Item2;
            }

            return default(TValue);
        }

        public TValue Get<TValue>(string key)
        {
            Guard.EnsureIsNotNullOrEmpty("key", key);

            var value = _values[key];

            return (TValue)value.Item2;
        }

        public bool HasValue(string key)
        {
            Guard.EnsureIsNotNullOrEmpty("key", key);

            return _values.ContainsKey(key);
        }

        public IEnumerable<object> Values()
        {
            return _values.Values;
        }
    }
}
