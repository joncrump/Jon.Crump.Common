using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;
using TightlyCurly.Com.Tests.Common.Helpers;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestClass]
    public class TheBuildInsertQueryMethod : MsTestMoqTestBase<SqlQueryBuilder>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                .Setup(x => x.GetBuilderStrategy(QueryKind.Insert))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.BuildInsertQuery<TestClass>(null, It.IsAny<bool>(), It.IsAny<bool>(),
                            It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                    .AndVerifyHasParameter("model");
            });
        }

        [TestMethod]
        public void WillInvokeQueryBuilderStrategyFactory()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.BuildInsertQuery(Mock.Of<TestClass>(), It.IsAny<bool>(),
                    It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>());

                Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                    .Verify(x => x.GetBuilderStrategy(QueryKind.Insert), Times.Once);
            });
        }
    }
}
