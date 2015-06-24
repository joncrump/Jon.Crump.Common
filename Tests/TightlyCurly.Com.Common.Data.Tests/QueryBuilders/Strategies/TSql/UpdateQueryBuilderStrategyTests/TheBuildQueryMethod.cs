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

namespace TightlyCurly.Com.Common.Data.Tests.QueryBuilders.Strategies.TSql.UpdateQueryBuilderStrategyTests
{
    [TestClass]
    public class TheBuildQueryMethod : MsTestMoqTestBase<UpdateQueryBuilderStrategy>
    {
        [TestMethod]
        public void WillBuildUpdateQueryWithPredicate()
        {
            QueryInfo expected = null;
            TestClass testClass = null;
            IEnumerable<IDbDataParameter> expectedParameters = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    testClass = ObjectCreator.CreateNew<TestClass>();
                    var whereClause = DataGenerator.GenerateString();
                    var expectedQuery = "UPDATE dbo.TestTable SET [TestClassId] = @id, [SomeFoo] = @fooParameter, [PioneerSquareBar] = @itsFridayLetsGoToTheBar WHERE {0};"
                        .FormatString(whereClause);
                    Mocks.Get<Mock<IPredicateBuilder>>()
                        .Setup(x => x.BuildContainer(It.IsAny<Expression<Func<TestClass, bool>>>(), It.IsAny<Type>(), true, It.IsAny<string>(), 
                            It.IsAny<string>()))
                        .Returns(new QueryContainer(whereClause, expectedParameters));
                    expectedParameters = new List<IDbDataParameter>
                    {
                        new SqlParameter("@id", SqlDbType.Int) {Value = testClass.Id},
                        new SqlParameter("@fooParameter", SqlDbType.NVarChar) {Value = testClass.Foo},
                        new SqlParameter("@itsFridayLetsGoToTheBar", SqlDbType.NVarChar) {Value = testClass.Bar}
                    };
                    expected = new QueryInfo(expectedQuery, Mock.Of<TableObjectMapping>(), expectedParameters);

                    Mocks.Get<Mock<IFieldHelper>>()
                        .Setup(x => x.BuildFields(It.IsAny<IEnumerable<string>>(), It.IsAny<string>(), testClass, It.IsAny<bool>(), 
                            It.IsAny<string>(), It.IsAny<string>()))
                        .Returns(new TableObjectMapping
                        {
                            TableName = "dbo.TestTable",
                            FieldMappings = new Dictionary<string, FieldParameterMapping>
                                {
                                    {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, testClass.Id)},
                                    {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, testClass.Foo)},
                                    {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", 
                                            SqlDbType.NVarChar, testClass.Bar)}
                                }
                        });

                    Mocks.Get<Mock<IFieldHelper>>()
                        .Setup(x => x.ExtractParameters(It.IsAny<TableObjectMapping>(), It.IsAny<bool>()))
                        .Returns(expectedParameters);
                })
                .ExecuteTest(() =>
                {
                    Expression<Func<TestClass, bool>> predicate = t => t.Bar == "Joe's Bar";
                    dynamic parameters = new ExpandoObject();
                    
                    parameters.Predicate = predicate;
                    parameters.Model = testClass;
                    parameters.CanDirtyRead = true;
                    parameters.IncludeParameters = null;
                    parameters.TableName = null;
                    parameters.DesiredFields = null;

                    QueryInfo actual = ItemUnderTest.BuildQuery<TestClass>(parameters);
                    Expression<Action<SqlParameter, SqlParameter>> expression = (e, a) => CompareParameters(e, a);

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "tableObjectMappings" });
                    Asserter.AssertEquality(expectedParameters.Select(p => (SqlParameter)p),
                        actual.Parameters.SafeSelect(p => (SqlParameter)p),
                        additionalParameters: new Dictionary<string, object>
                        {
                            {
                                Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                                expression
                            }
                        });
                });
        }

        private void CompareParameters(SqlParameter expected, SqlParameter actual)
        {
            Asserter.AssertEquality(expected, actual, new[]
            {
                "Value", "SqlDbType", "DbType", "SqlValue", "SourceVersion",
                "CompareInfo", "Direction"
            });
            Asserter.AssertEquality(expected.SqlDbType, actual.SqlDbType);
        }
    }
}
