using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class StringAsserterStategy : AsserterStrategyBase<String>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.StringAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(string); }
        }

        public StringAsserterStategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }

        public override void AssertEquality(string expected, string actual, 
            IEnumerable<string> propertiesToIgnore = null,
            IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters,
                Constants.ParameterNames.CheckForDefault);
            var checkForNullOrEmpty = ExtractParameter<bool>(additionalParameters,
                Constants.ParameterNames.CheckForNullOrEmpty);

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            if (checkForNullOrEmpty)
            {
                Assert.IsFalse(String.IsNullOrEmpty(actual));
            }

            if (String.Compare(expected, actual, StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new AssertException(String.Format("Strings are not equal.\n Expected: {0}\n Actual: {1}",
                    expected, actual));
            }
        }
    }
}
