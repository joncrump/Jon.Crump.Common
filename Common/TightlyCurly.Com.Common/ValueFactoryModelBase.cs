using System;
using System.Collections.Generic;
using System.Linq;

namespace TightlyCurly.Com.Common
{
    public abstract class ValueFactoryModelBase
    {
        public IDictionary<string, Func<object>> ValueFactories { get; set; }
        private readonly List<string> _loadedValues = new List<string>();

        protected ValueFactoryModelBase()
        {
            ValueFactories = new Dictionary<string, Func<object>>();
        }

        protected TValue GetOrLoadLazyValue<TValue>(TValue value, string key) where TValue : class
        {
            if (value == null && !HasBeenLoaded(key))
            {
                value = HydrateValue<TValue>(key);
            }

            return value;
        }

        private TValue HydrateValue<TValue>(string key)
        {
            var objectToHydrate = default(TValue);

            if (ValueFactories == null)
            {
                ValueFactories = new Dictionary<string, Func<object>>();
            }

            if (ValueFactories.ContainsKey(key))
            {
                var valueFactory = ValueFactories[key];

                if (valueFactory != null)
                {
                    objectToHydrate = (TValue)valueFactory();
                }
            }

            if (!_loadedValues.Contains(key))
            {
                _loadedValues.Add(key);
            }

            return objectToHydrate;
        }

        private bool HasBeenLoaded(string key)
        {
            return _loadedValues.Any(v => v == key);
        }
    }
}
