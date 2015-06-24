using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Tests.ReadDatabaseRepositoryBaseTests;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.ReadDatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheGetMethod : MsTestMoqRepositoryBase<TestableReadDatabaseRepository>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IQueryBuilder>>()
                .Setup(x => x.BuildSelectQuery(It.IsAny<Expression<Func<TestModel, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(), It.IsAny<BuildMode>()))
                .Returns(Mock.Of<QueryInfo>());
        }

        [TestMethod]
        public void WillInvokeQueryBuilderBuildSelectQuery()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    ItemUnderTest.Get();

                    Mocks.Get<Mock<IQueryBuilder>>()
                        .Verify(x => x.BuildSelectQuery(It.IsAny<Expression<Func<TestModel, bool>>>(), 
                            It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), 
                            It.IsAny<BuildMode>()), Times.Once);
                });
        }
    }
}
