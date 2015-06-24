using System.Linq.Expressions;
using TightlyCurly.Com.Tests.Common.Base;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common.MsTest
{
    public class MsTestBase : TestBase
    {
        public MsTestBase() : base(new MsTestAssertAdapter())
        {
        }

        public MsTestBase(IDataGenerator dataGenerator) : base(dataGenerator, new MsTestAssertAdapter())
        {
        }

        public MsTestBase(IObjectCreator objectCreator) : base(objectCreator, new MsTestAssertAdapter())
        {
        }

        public MsTestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator)
            : base(dataGenerator, objectCreator, new MsTestAssertAdapter())
        {
        }
    }
}
