using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class UriAsserterStrategy : AsserterStrategyBase<Uri>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.UriAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(Uri); }
        }

        public UriAsserterStrategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }

        public override void AssertEquality(Uri expected, Uri actual, IEnumerable<string> propertiesToIgnore = null, IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters,
                Constants.ParameterNames.CheckForDefault);

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
