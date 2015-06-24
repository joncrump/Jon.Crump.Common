using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public interface IAsserterStrategy
    {
        string Name { get; }
        Type Type { get; }
        void AssertEquality(object expected, object actual, IEnumerable<string> propertiesToIgnore = null, 
             IDictionary<string, object> additionalParameters = null, bool recurseProperties = false);
    }

    public interface IAsserterStrategy<in TValue> : IAsserterStrategy
    {
        void AssertEquality(TValue expected, TValue actual, IEnumerable<string> propertiesToIgnore = null, 
             IDictionary<string, object> additionalParameters = null, bool recurseProperties = false);
    }
}
