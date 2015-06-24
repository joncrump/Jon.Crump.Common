using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class ByteAsserterStrategy : AsserterStrategyBase<byte>
    {
        public override string Name
        {
            get
            {
                return Constants.StrategyNames.ByteAsserterStrategyName;
            }
        }

        public override Type Type
        {
            get { return typeof(byte); }
        }

        public ByteAsserterStrategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }
        
        public override void AssertEquality(byte expected, byte actual, IEnumerable<string> propertiesToIgnore = null, 
            IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);
            
            if (checkForDefault)
            {
                CheckForDefault<byte>(actual);
            }

            Assert.AreEqual(expected, actual);
        }
    }
}
