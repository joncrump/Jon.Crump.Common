using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.Strategies.DictionaryBasedDataReaderJoinedBuilderStrategyTests
{
    [TestClass]
    public class TheConstructor : MsTestBase
    {
        [TestMethod]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<DictionaryBasedDataReaderJoinedBuilderStrategy>();
        }
    }
}
