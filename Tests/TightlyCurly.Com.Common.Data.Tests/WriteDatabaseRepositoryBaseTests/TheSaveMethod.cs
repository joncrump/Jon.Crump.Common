using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheSaveMethod : MsTestMoqRepositoryBase<TestableWriteDatabaseRepository>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IMapper>>()
                .Setup(x => x.Map<TestModel>(It.IsAny<ITestModel>()))
                .Returns(ObjectCreator.CreateNew<TestModel>());

            Mocks.Get<Mock<IMapper>>()
                .Setup(x => x.Map<TestModel>(It.IsAny<ITestModel>()))
                .Returns(ObjectCreator.CreateNew<TestModel>());

            Mocks.Get<Mock<IQueryBuilder>>()
                .Setup(x => x.BuildInsertQuery(It.IsAny<TestModel>(), It.IsAny<bool>(),
                    It.IsAny<bool>(), It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .Returns(Mock.Of<QueryInfo>());

            Mocks.Get<Mock<IQueryBuilder>>()
                .Setup(x => x.BuildUpdateQuery(It.IsAny<TestModel>(), It.IsAny<Expression<Func<TestModel, bool>>>(),
                    It.IsAny<IEnumerable<string>>(), It.IsAny<string>()))
                .Returns(Mock.Of<QueryInfo>());
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.Save(null, It.IsAny<bool>(),
                            It.IsAny<Action<ITestModel>>(), It.IsAny<Action<ITestModel>>(),
                            It.IsAny<Expression>()))
                    .AndVerifyHasParameter("model");
            });
        }

        [TestMethod]
        public void WillInvokeMapper()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.Save(Mock.Of<ITestModel>(), true, null, null, null);

                Mocks.Get<Mock<IMapper>>()
                    .Verify(x => x.Map<TestModel>(It.IsAny<ITestModel>()), 
                        Times.Once);
            });
        }

        [TestMethod]
        public void WillInvokeInsertActionIfIsNew()
        {
            var invoked = false;
            Action<ITestModel> insertAction = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    insertAction = t => invoked = true;
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.Save(Mock.Of<ITestModel>(), true, insertAction, null, null);

                    Assert.IsTrue(invoked);
                });
        }

        [TestMethod]
        public void WillInvokeQueryBuilderInsertQuery()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.Save(Mock.Of<ITestModel>(), true, It.IsAny<Action<ITestModel>>(), 
                    It.IsAny<Action<ITestModel>>(), It.IsAny<Expression>());

                Mocks.Get<Mock<IQueryBuilder>>()
                    .Verify(x => x.BuildInsertQuery(It.IsAny<TestModel>(), false, It.IsAny<bool>(), 
                        It.IsAny<IEnumerable<string>>(), It.IsAny<string>()), Times.Once);
            });
        }

        [TestMethod]
        public void WillInvokeExecuteNonQueryIfModelIsNew()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.Save(Mock.Of<ITestModel>(), true, It.IsAny<Action<ITestModel>>(),
                    It.IsAny<Action<ITestModel>>(), It.IsAny<Expression>());

                MockDatabase
                    .Verify(x => x.ExecuteNonQuery(), Times.Once);
            });
        }

        [TestMethod]
        public void WillInvokeUpdateActionIfNotIsNew()
        {
            Action<ITestModel> updateAction = null;
            var invoked = false;
            Expression updateExpression = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    updateAction = t => invoked = true;
                    updateExpression = Expression.Parameter(typeof (TestModel));
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.Save(Mock.Of<ITestModel>(), false, null, updateAction,
                        updateExpression);

                    Assert.IsTrue(invoked);
                });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfUpdateExpressionIsNullAndIsNewIsFalse()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<InvalidOperationException>(
                        () => ItemUnderTest.Save(Mock.Of<ITestModel>(), false, null, null, null))
                    .AndVerifyMessageContains("Don't know how to update model.  Update expression is null");
            });
        }

        [TestMethod]
        public void WillInvokeQueryBuilderBuildUpdateQuery()
        {
            Expression updateExpression = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    updateExpression = Expression.Parameter(typeof (TestModel));
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.Save(Mock.Of<ITestModel>(), false, null, null, updateExpression);

                    Mocks.Get<Mock<IQueryBuilder>>()
                        .Verify(x => x.BuildUpdateQuery(It.IsAny<TestModel>(), It.IsAny<Expression<Func<TestModel, bool>>>(), 
                            It.IsAny<IEnumerable<string>>(), It.IsAny<string>()), 
                                Times.Once);
                });
        }
    }
}
