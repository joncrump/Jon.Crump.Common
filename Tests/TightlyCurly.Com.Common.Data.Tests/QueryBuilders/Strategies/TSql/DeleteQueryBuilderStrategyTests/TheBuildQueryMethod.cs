using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.QueryBuilders.Strategies.TSql.DeleteQueryBuilderStrategyTests
{
    [TestClass]
    public class TheBuildQueryMethod : MsTestMoqTestBase<DeleteQueryBuilderStrategy>
    {
        public override void Setup()
        {
            base.Setup();

            Mocks.Get<Mock<IFieldHelper>>()
                .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(),
                    It.IsAny<DummyClass>(), It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new TableObjectMapping
                {
                    TableName = DataGenerator.GenerateString(),
                    FieldMappings = new Dictionary<string, FieldParameterMapping>()
                });
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfParametersAreNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.BuildQuery<TestClass>(null))
                    .AndVerifyHasParameter("parameters");
            });
        }

        [TestMethod]
        public void WillBuildDeleteQueryWithPredicate()
        {
            QueryInfo expected = null;
            TestClass testClass;
            IEnumerable<IDbDataParameter> expectedParameters = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    testClass = ObjectCreator.CreateNew<TestClass>();
                    var whereClause = DataGenerator.GenerateString();
                    var expectedQuery = "DELETE FROM dbo.TestTable WHERE {0};"
                        .FormatString(whereClause);
                    expectedParameters = new List<IDbDataParameter>
                    {
                        new SqlParameter("@id", SqlDbType.Int) {Value = testClass.Id},
                    };
                    Mocks.Get<Mock<IPredicateBuilder>>()
                        .Setup(x => x.BuildContainer(It.IsAny<Expression<Func<TestClass, bool>>>(), It.IsAny<Type>(), true, It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(new QueryContainer(whereClause, expectedParameters));

                    expected = new QueryInfo(expectedQuery, It.IsAny<TableObjectMapping>(), expectedParameters);

                    Mocks.Get<Mock<IFieldHelper>>()
                        .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), It.IsAny<TestClass>(), It.IsAny<bool>(), 
                            It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(new TableObjectMapping
                        {
                            TableName = "dbo.TestTable",
                            FieldMappings = new Dictionary<string, FieldParameterMapping>
                                {
                                    {"Id", new FieldParameterMapping("Id", "@id", SqlDbType.Int)}
                                }
                        });
                })
                .ExecuteTest(() =>
                {
                    Expression<Func<TestClass, bool>> predicate = t => t.Bar == "Joe's Bar";
                    dynamic parameters = new ExpandoObject();

                    parameters.Predicate = predicate;
                    parameters.TableName = null;

                    QueryInfo actual = ItemUnderTest.BuildQuery<TestClass>(parameters);
                    Expression<Action<SqlParameter, SqlParameter>> expression =
                        (e, a) => CompareSqlParameters(e, a);

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });
                    Asserter.AssertEquality(expectedParameters.Select(p =>
                    {
                        var s = (SqlParameter)p;
                        return new { s.ParameterName, s.Value, s.SqlDbType };
                    }),
                        actual.Parameters.SafeSelect(p =>
                        {
                            var s = (SqlParameter)p;
                            return new { s.ParameterName, s.Value, s.SqlDbType };
                        }),
                    propertiesToIgnore: new[] {"Value", "SqlDbType"},
                    additionalParameters: new Dictionary<string, object>
                    {
                        {
                            Com.Tests.Common.Constants.ParameterNames.AssertDelegate,
                            expression
                        }
                    });
                });
        }

        private void CompareSqlParameters(SqlParameter expected, SqlParameter actual)
        {
            Asserter.AssertEquality(expected, actual, propertiesToIgnore: new[] { "Value", "SqlDbType" });
            Asserter.AssertEquality(expected.SqlDbType, actual.SqlDbType);
        }
    }
}
