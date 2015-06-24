using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestClass]
    public class TheBuildPagedQueryMethod : MsTestMoqTestBase<SqlQueryBuilder>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                .Setup(x => x.GetBuilderStrategy(It.IsAny<QueryKind>()))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [TestMethod]
        public void WillInvokeSelectSingleStrategyIfPagingInfoIsNull()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.BuildPagedQuery<TestClass>(null);

                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                        .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Once);
                });
        }

        [TestMethod]
        public void WillNotInvokeSelectSingleStrategyIfPagingInfoIsNotNull()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.BuildPagedQuery<TestClass>(ObjectCreator.CreateNew<PagingInfo>());

                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                       .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), Times.Never());
                });
        }

        [TestMethod]
        public void WillInvokePagedSingleStrategy()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.BuildPagedQuery<TestClass>(ObjectCreator.CreateNew<PagingInfo>());

                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                        .Verify(x => x.GetBuilderStrategy(QueryKind.PagedSingle), Times.Once);
                });
        }
    }
}
