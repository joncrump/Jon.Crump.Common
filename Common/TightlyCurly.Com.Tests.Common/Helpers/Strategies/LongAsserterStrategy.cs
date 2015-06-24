using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class LongAsserterStrategy : AsserterStrategyBase<long>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.LongAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(long); }
        }

        public LongAsserterStrategy(IAssertAdapter assertAdapter)
            : base(assertAdapter)
        {
        }
        
        public override void AssertEquality(long expected, long actual, IEnumerable<string> propertiesToIgnore = null, 
            IDictionary<string, object> additionalParameters = null, bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);
            var checkForNonZero = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForNonZero);
            var checkForPositive = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForPositive);

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            if (checkForNonZero)
            {
                Assert.AreNotEqual(0, actual);
            }

            if (checkForPositive)
            {
                Assert.IsTrue(actual >= 0);
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
