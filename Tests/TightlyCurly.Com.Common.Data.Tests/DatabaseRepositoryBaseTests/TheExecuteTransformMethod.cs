using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheExecuteTransformMethod : MsTestMoqRepositoryBase<TestableDatabaseRepository>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfQueryInfoIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () =>
                            ItemUnderTest.ExecuteTransform(null, It.IsAny<Func<IDataReader, object>>(),
                                It.IsAny<Action<object>>()))
                    .AndVerifyHasParameter("queryInfo");

            });
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfBuilderDelegateIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), null,
                            It.IsAny<Action<object>>()))
                    .AndVerifyHasParameter("builderDelegate");
            });
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfTransformActionIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Func<IDataReader, object> builderDelegate = r => r;

                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), builderDelegate,
                            null))
                    .AndVerifyHasParameter("transformAction");
            });
        }

        [TestMethod]
        public void WillInvokeDatabaseCreateCommandTextMethod()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    Func<IDataReader, object> builderDelegate = r => r;
                    Action<object> transformAction = FakeMethod;

                    ItemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), builderDelegate, 
                        transformAction);

                    MockDatabase
                        .Verify(x => x.CreateCommandText(It.IsAny<string>(), QueryType.Text), 
                            Times.Once);
                });
        }

        [TestMethod]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreNull()
        {
            QueryInfo queryInfo = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    queryInfo = new QueryInfo {Parameters = null};
                })
                .ExecuteTest(() =>
                {
                    Func<IDataReader, object> builderDelegate = r => r;
                    Action<object> transformAction = FakeMethod;

                    ItemUnderTest.ExecuteTransform(queryInfo, builderDelegate, transformAction);

                    MockDatabase
                        .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                            Times.Never);
                });
        }

        [TestMethod]
        public void WillNotInvokeDatabaseWithParametersMethodIfParametersAreEmpty()
        {
            QueryInfo queryInfo = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    queryInfo = new QueryInfo
                    {
                        Parameters = Enumerable.Empty<IDbDataParameter>()
                    };
                })
                .ExecuteTest(() =>
                {
                    Func<IDataReader, object> builderDelegate = r => r;
                    Action<object> transformAction = FakeMethod;

                    ItemUnderTest.ExecuteTransform(queryInfo, builderDelegate, transformAction);

                    MockDatabase
                        .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                            Times.Never);
                });
        }

        [TestMethod]
        public void WillInvokeDatabaseWithParametersMethodIfParametersHaveValues()
        {
            QueryInfo queryInfo = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    queryInfo = new QueryInfo
                    {
                        Parameters = new List<SqlParameter>
                        {
                            new SqlParameter()
                        }
                    };
                })
                .ExecuteTest(() =>
                {
                    Func<IDataReader, object> builderDelegate = r => r;
                    Action<object> transformAction = FakeMethod;

                    ItemUnderTest.ExecuteTransform(queryInfo, builderDelegate, transformAction);

                    MockDatabase
                        .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                            Times.Once);
                });
        }

        [TestMethod]
        public void WillInvokeDatabaseExecuteTransformMethod()
        {
            TestRunner.ExecuteTest(() =>
            {
                Func<IDataReader, object> builderDelegate = r => r;
                Action<object> transformAction = FakeMethod;

                ItemUnderTest.ExecuteTransform(Mock.Of<QueryInfo>(), 
                    builderDelegate, transformAction);

                MockDatabase
                    .Verify(x => x.ExecuteTransform(It.IsAny<Func<IDataReader, object>>(), 
                        It.IsAny<Action<object>>()), Times.Once);
            });
        }

        private void FakeMethod(object value)
        {
        }
    }
}
