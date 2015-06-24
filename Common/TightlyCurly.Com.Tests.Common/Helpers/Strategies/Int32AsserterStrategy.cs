using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class Int32AsserterStrategy : AsserterStrategyBase<Int32>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.Int32AsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(Int32); }
        }

        public Int32AsserterStrategy(IAssertAdapter assertAdapter)
            : base(assertAdapter)
        {
        }

        public override void AssertEquality(int expected, int actual, IEnumerable<string> propertiesToIgnore = null, IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
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
