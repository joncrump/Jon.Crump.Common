using TightlyCurly.Com.Tests.Common.Base;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common.MsTest
{
    public class MsTestMoqTestBase<TItemUnderTest> : MoqTestBase<TItemUnderTest> where TItemUnderTest : class
    {
        public MsTestMoqTestBase() : base(new MsTestAssertAdapter())
        {
        }

        public MsTestMoqTestBase(IDataGenerator dataGenerator) 
            : base(dataGenerator, new MsTestAssertAdapter())
        {
        }

        public MsTestMoqTestBase(IObjectCreator objectCreator) : base(objectCreator, new MsTestAssertAdapter())
        {
        }

        public MsTestMoqTestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator)
            : base(dataGenerator, objectCreator, new MsTestAssertAdapter())
        {
        }
    }
}
