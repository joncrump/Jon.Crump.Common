using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestClass]
    public class TheBuildCountQueryMethod : MsTestMoqTestBase<SqlQueryBuilder>
    {
        [TestMethod]
        public void WillInvokeQueryBuilderStrategyFactory()
        {
            TestRunner
                .DoCustomSetup(() =>
                {
                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                        .Setup(x => x.GetBuilderStrategy(QueryKind.Count))
                        .Returns(new Mock<IQueryBuilderStrategy>().Object);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.BuildCountQuery<TestClass>();

                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                        .Verify(x => x.GetBuilderStrategy(QueryKind.Count), Times.Once);
                });
        }
    }
}
