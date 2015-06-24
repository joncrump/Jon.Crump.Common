using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheDeleteMethod : MsTestMoqRepositoryBase<TestableWriteDatabaseRepository>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IQueryBuilder>>()
                .Setup(x => x.BuildDeleteQuery(It.IsAny<Expression<Func<TestModel, bool>>>(), 
                    It.IsAny<string>()))
                .Returns(Mock.Of<QueryInfo>());
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.Delete(null, It.IsAny<Expression<Func<TestModel, bool>>>()))
                    .AndVerifyHasParameter("model");
            });
        }

        [TestMethod]
        public void WillInvokeQueryBuilderDeleteQuery()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.Delete(Mock.Of<ITestModel>());

                Mocks.Get<Mock<IQueryBuilder>>()
                    .Verify(x => x.BuildDeleteQuery<ITestModel>(null, null), Times.Once);
            });
        }
    }
}
