using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class NullableAsserterStrategy<TValue> : AsserterStrategyBase<TValue?> where TValue : struct 
    {
        public override string Name
        {
            get { return Constants.StrategyNames.NullableAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(TValue?); }
        }

        public NullableAsserterStrategy(IAssertAdapter assertAdapter)
            : base(assertAdapter)
        {
        }

        public override void AssertEquality(TValue? expected, TValue? actual, IEnumerable<string> propertiesToIgnore = null,
           IDictionary<string, object> additionalParameters = null, bool recurseProperties = false)
        {
            var assertDelegate = ExtractParameter<Expression<Action<TValue, TValue>>>(
                additionalParameters, Constants.ParameterNames.AssertDelegate);

            if (expected.HasValue)
            {
                Assert.IsTrue(actual.HasValue);
            }
            else
            {
                Assert.IsFalse(actual.HasValue);
            }

            if (assertDelegate != null && expected.HasValue)
            {
                assertDelegate.Compile()(expected.Value, actual.Value);
            }
        }
    }
}
