using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheExecuteNonQueryMethod : MsTestMoqRepositoryBase<TestableDatabaseRepository>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfQueryInfoIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.ExecuteNonQuery(null))
                    .AndVerifyHasParameter("queryInfo");
            });
        }

        [TestMethod]
        public void WillInvokeDatabaseCreateCommandTextMethod()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.ExecuteNonQuery(Mock.Of<QueryInfo>());

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
                    queryInfo = new QueryInfo { Parameters = null };
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.ExecuteNonQuery(queryInfo);

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
                    ItemUnderTest.ExecuteNonQuery(queryInfo);

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
                    ItemUnderTest.ExecuteNonQuery(queryInfo);

                    MockDatabase
                        .Verify(x => x.WithParameters(It.IsAny<IEnumerable<IDbDataParameter>>()),
                            Times.Once);
                });
        }

        [TestMethod]
        public void WillInvokeDatabaseExecuteNonQueryMethod()
        {
            TestRunner.ExecuteTest(() =>
            {
                ItemUnderTest.ExecuteNonQuery(Mock.Of<QueryInfo>());

                MockDatabase
                    .Verify(x => x.ExecuteNonQuery(), Times.Once);
            });
        }
    }
}
