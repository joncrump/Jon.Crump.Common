using System.Collections.Generic;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Tests.ObjectStateManagerTests
{
    public class TestableObjectStateManager : ObjectStateManager
    {
        public IDictionary<string, InitialObjectState> ObjectStates
        {
            get { return InitialObjectStates; }
            set { InitialObjectStates = value; }
        }

        public TestableObjectStateManager(IHashHelper hashHelper, 
            IStateStore stateStore) : base(hashHelper, stateStore)
        {
        }
    }
}
