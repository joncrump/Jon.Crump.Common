using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class EnumAsserterStrategy : AsserterStrategyBase<Enum>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.EnumAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(Enum); }
        }

        public EnumAsserterStrategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }

        public override void AssertEquality(Enum expected, Enum actual, IEnumerable<string> propertiesToIgnore = null, IDictionary<string, object> additionalParameters = null, 
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
