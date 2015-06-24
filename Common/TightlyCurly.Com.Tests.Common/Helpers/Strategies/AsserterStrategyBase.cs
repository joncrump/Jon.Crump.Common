using System;
using System.Collections.Generic;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public abstract class AsserterStrategyBase<TValue> : IAsserterStrategy<TValue>
    {
        protected IAssertAdapter Assert { get; private set; }
        
        public abstract string Name { get; }
        public abstract Type Type { get; }

        protected AsserterStrategyBase(IAssertAdapter assertAdapter)
        {
            Assert = assertAdapter;
        }

        public abstract void AssertEquality(TValue expected, TValue actual, IEnumerable<string> propertiesToIgnore = null,
            IDictionary<string, object> additionalParameters = null, bool recurseProperties = false);

        public virtual void AssertEquality(object expected, object actual, IEnumerable<string> propertiesToIgnore = null,
            IDictionary<string, object> additionalParameters = null, bool recurseProperties = false)
        {
            AssertEquality((TValue)expected, (TValue)actual, propertiesToIgnore, additionalParameters);
        }

        protected TModel ExtractParameter<TModel>(IDictionary<string, object> parameters, 
            string key)
        {
            if (parameters.IsNullOrEmpty() || !parameters.ContainsKey(key))
            {
                return default(TModel);
            }

            return (TModel)parameters[key];
        }

        protected void CheckForDefault<TModel>(TModel value)
        {
            Assert.AreNotEqual(default(TModel), value, "Parameter should not be default");
        }
    }
}
