using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class BooleanAsserterStrategy : AsserterStrategyBase<bool>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.BooleanAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(bool); }
        }

        public BooleanAsserterStrategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }

        public override void AssertEquality(bool expected, bool actual, IEnumerable<string> propertiesToIgnore = null, IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
