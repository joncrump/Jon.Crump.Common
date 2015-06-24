using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.SqlQueryBuilderTests
{
    [TestClass]
    public class TheBuildSelectQueryMethod : MsTestMoqTestBase<SqlQueryBuilder>
    {
        public override void Setup()
        {
            base.Setup();

            var factory = Mocks.Get<Mock<IQueryBuilderStrategyFactory>>();

            factory
                .Setup(x => x.GetBuilderStrategy(It.IsAny<QueryKind>()))
                .Returns(new Mock<IQueryBuilderStrategy>().Object);
        }

        [TestMethod]
        public void WillInvokeBuilderStrategyIfBuildModeIsSingle()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.BuildSelectQuery(It.IsAny<Expression<Func<TestClass, bool>>>(),
                        It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(),
// ReSharper disable once RedundantArgumentDefaultValue
                        It.IsAny<string>(), BuildMode.Single);

                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                        .Verify(x => x.GetBuilderStrategy(QueryKind.SelectSingleTable), 
                            Times.Once);
                });
        }

        [TestMethod]
        public void WillInvokeBuilderStrategyIfBuildModeIsJoined()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.BuildSelectQuery(It.IsAny<Expression<Func<TestClass, bool>>>(),
                        It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(),
                        It.IsAny<string>(), BuildMode.Joined);

                    Mocks.Get<Mock<IQueryBuilderStrategyFactory>>()
                        .Verify(x => x.GetBuilderStrategy(QueryKind.SelectJoinTable),
                            Times.Once);
                });
        }
    }
}
