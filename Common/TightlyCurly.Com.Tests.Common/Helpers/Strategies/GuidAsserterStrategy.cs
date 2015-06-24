using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class GuidAsserterStrategy : AsserterStrategyBase<Guid>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.GuidAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(Guid); }
        }

        public GuidAsserterStrategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }

        public override void AssertEquality(Guid expected, Guid actual, IEnumerable<string> propertiesToIgnore = null, IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters,
                Constants.ParameterNames.CheckForDefault);
            var checkForEmpty = ExtractParameter<bool>(additionalParameters,
                Constants.ParameterNames.CheckForEmptyGuid);

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            if (checkForEmpty)
            {
                Assert.AreNotEqual(Guid.Empty, actual);
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
