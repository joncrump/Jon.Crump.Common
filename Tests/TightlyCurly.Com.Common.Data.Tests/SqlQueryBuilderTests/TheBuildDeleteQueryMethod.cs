using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestClass]
    public class TheBuildDeleteQueryMethod : MsTestMoqTestBase<SqlQueryBuilder>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.Delete))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfPredicateIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.BuildDeleteQuery<TestClass>(null, It.IsAny<string>()))
                    .AndVerifyMessageContains("predicate");
            });
        }

        [TestMethod]
        public void WillInvokeQueryBuilderStrategy()
        {
            TestRunner.ExecuteTest(() =>
            {
                Expression<Func<TestClass, bool>> predicate = t => t.Id == 5;

                ItemUnderTest.BuildDeleteQuery(predicate);

                Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                    .Verify(x => x.GetBuilderStrategy(QueryKind.Delete), Times.Once);
            });
        }
    }
}