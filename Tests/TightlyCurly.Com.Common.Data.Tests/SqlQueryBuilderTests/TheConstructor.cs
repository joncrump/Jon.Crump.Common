using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestClass]
    public class TheConstructor : MsTestBase
    {
        [TestMethod]
        public void WillPassConstructorTests()
        {
            DoConstructorTests<SqlQueryBuilder>();
        }
    }
}
