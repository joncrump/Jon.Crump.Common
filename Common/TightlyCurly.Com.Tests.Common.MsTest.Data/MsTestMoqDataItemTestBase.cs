using TightlyCurly.Com.Tests.Common.Data;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common.MsTest.Data
{
    public abstract class MsTestMoqDataItemTestBase<TItemUnderTest> : MoqDataItemTestBase<TItemUnderTest>
        where TItemUnderTest : class
    {
        protected MsTestMoqDataItemTestBase() : base(new MsTestAssertAdapter(), new MockDataReaderHelper())
        {
        }

        protected MsTestMoqDataItemTestBase(IDataGenerator dataGenerator, IDataReaderHelper dataReaderHelper) : base(dataGenerator, new MsTestAssertAdapter(), dataReaderHelper)
        {
        }

        protected MsTestMoqDataItemTestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator, IDataReaderHelper dataReaderHelper) : base(dataGenerator, objectCreator, new MsTestAssertAdapter(), dataReaderHelper)
        {
        }

        protected MsTestMoqDataItemTestBase(IObjectCreator objectCreator, IDataReaderHelper dataReaderHelper) : base(objectCreator, new MsTestAssertAdapter(), dataReaderHelper)
        {
        }
    }
}
