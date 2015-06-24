using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.DatabaseQueryPredicateBuilderTests
{
    [TestClass]
    public class TheBuildContainerMethod : MsTestMoqTestBase<DatabaseQueryPredicateBuilder>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfExpressionIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.BuildContainer(null, It.IsAny<Type>()))
                    .AndVerifyHasParameter("expression");
            });
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfTypeIsNull()
        {
            Expression<Func<TestClass, bool>> expression = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var id = DataGenerator.GenerateInteger();
                    var testClass = ObjectCreator.CreateNew<TestClass>();
                    expression = t => t.Id == id && t.Bar == testClass.Bar || t.Foo == "Margaritas";
                })
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<ArgumentNullException>(
                            () => ItemUnderTest.BuildContainer(expression, null))
                        .AndVerifyHasParameter("declaringType");
                });
        }

        [TestMethod]
        public void WillBuildContainer()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var id = DataGenerator.GenerateInteger();
                    var testClass = ObjectCreator.CreateNew<TestClass>();
                    expression = t => t.Id == id && t.Bar == testClass.Bar || t.Foo == "Margaritas";

                    expected = new QueryContainer(
                        "TestClassId = @id AND PioneerSquareBar = @itsFridayLetsGoToTheBar OR SomeFoo = @fooParameter",
                        new List<IDbDataParameter>
                        {
                            new SqlParameter
                            {
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Value = id
                            },
                            new SqlParameter
                            {
                                ParameterName = "@itsFridayLetsGoToTheBar",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = testClass.Bar
                            },
                            new SqlParameter
                            {
                                ParameterName = "@fooParameter",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = "Margaritas"
                            }
                        });
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass));
                    Expression<Action<SqlParameter, SqlParameter>> compareExpression =
                        (e, a) => CompareParameters(e, a);

                    Asserter.AssertEquality(expected, actual, new[] {"Parameters", "OrderByClause"});

                    Asserter.AssertEquality(
                        expected.Parameters.Select(p => (SqlParameter)p), 
                        actual.Parameters.Select(p => (SqlParameter)p), 
                        additionalParameters:new Dictionary<string, object>
                        {
                            {
                                Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                                compareExpression
                            }
                        });
                });
        }

        [TestMethod]
        public void WillBuildContainerWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var id = DataGenerator.GenerateInteger();
                    var testClass = ObjectCreator.CreateNew<TestClass>();
                    expression = t => t.Id == id && t.Bar == testClass.Bar || t.Foo == "Margaritas";

                    expected = new QueryContainer(
                        "t1.t1_TestClassId = @id AND t1.t1_PioneerSquareBar = @itsFridayLetsGoToTheBar OR t1.t1_SomeFoo = @fooParameter",
                        new List<IDbDataParameter>
                        {
                            new SqlParameter
                            {
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Value = id
                            },
                            new SqlParameter
                            {
                                ParameterName = "@itsFridayLetsGoToTheBar",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = testClass.Bar
                            },
                            new SqlParameter
                            {
                                ParameterName = "@fooParameter",
                                SqlDbType = SqlDbType.NVarChar,
                                Value = "Margaritas"
                            }
                        });
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), tableAlias:"t1", fieldPrefix:"t1_");
                    Expression<Action<SqlParameter, SqlParameter>> compareExpression =
                        (e, a) => CompareParameters(e, a);

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });

                    Asserter.AssertEquality(
                        expected.Parameters.Select(p => (SqlParameter)p),
                        actual.Parameters.Select(p => (SqlParameter)p),
                        additionalParameters: new Dictionary<string, object>
                        {
                            {
                                Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                                compareExpression
                            }
                        });
                });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfPropertyHasNoMetadataAttributes()
        {
            DummyClass parameterClass = null;
            DummyClass comparisonClass = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    parameterClass = ObjectCreator.CreateNew<DummyClass>();
                    comparisonClass = ObjectCreator.CreateNew<DummyClass>();
                })
                .ExecuteTest(() =>
                {
                    Expression<Func<DummyClass, bool>> expression = d => d.SomeProperty == comparisonClass.SomeProperty;

                    Asserter
                        .AssertExceptionIsThrown<InvalidOperationException>(
                            () => ItemUnderTest.BuildContainer(expression, typeof(TestClass)))
                        .AndVerifyMessageContains("Cannot build query.  Property has no metadata attributes: SomeProperty");
                });
        }

        [TestMethod]
        public void WillHandleStartsWithClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.StartsWith("Hink");

                    expected = new QueryContainer(
                        "PioneerSquareBar LIKE ('Hink%')");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleStartsWithClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.StartsWith("Hink");

                    expected = new QueryContainer(
                        "t1.t1_PioneerSquareBar LIKE ('Hink%')");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), 
                        tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleContainsClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.Contains("yikes");

                    expected = new QueryContainer(
                        "PioneerSquareBar LIKE ('%yikes%')");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleContainsClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.Contains("yikes");

                    expected = new QueryContainer(
                        "t1.t1_PioneerSquareBar LIKE ('%yikes%')");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), 
                        tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleEndsWithClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.EndsWith("yikes");

                    expected = new QueryContainer(
                        "PioneerSquareBar LIKE ('%yikes')");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleEndsWithClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.EndsWith("yikes");

                    expected = new QueryContainer(
                        "t1.t1_PioneerSquareBar LIKE ('%yikes')");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), 
                        tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleStringEqualsClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.Equals("yikes");

                    expected = new QueryContainer(
                        "PioneerSquareBar = 'yikes'");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleStringEqualsClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.Equals("yikes");

                    expected = new QueryContainer(
                        "t1.t1_PioneerSquareBar = 'yikes'");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), 
                        tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleIntEqualsClauses()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Id.Equals(5);

                    expected = new QueryContainer(
                        "TestClassId = 5");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleIntEqualsClausesWithPrefix()
        {
            Expression<Func<TestClass, bool>> expression = null;
            QueryContainer expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Id.Equals(5);

                    expected = new QueryContainer(
                        "t1.t1_TestClassId = 5");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), 
                        tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleEqualsNullClause()
        {
            QueryContainer expected = null;
            Expression<Func<TestClass, bool>> expression = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.Equals(null);

                    expected = new QueryContainer("PioneerSquareBar IS NULL");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof (TestClass));

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleEqualsNullClauseWithPrefix()
        {
            QueryContainer expected = null;
            Expression<Func<TestClass, bool>> expression = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expression = t => t.Bar.Equals(null);

                    expected = new QueryContainer("t1.t1_PioneerSquareBar IS NULL");
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(TestClass), 
                        tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(expected, actual, new[] { "Parameters", "OrderByClause" });
                });
        }


        [TestMethod]
        public void WillHandleDateTime()
        {
            Expression expression = null;
            string expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var dateClass = ObjectCreator.CreateNew<DateClass>();

                    var parameterExpression = Expression.Parameter(typeof (DateClass), "d");
                    var propertyInfo = typeof (DateClass).GetProperty("SomeDateTime");
                    var memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
                    var dateValue = dateClass.SomeDateTime;

                    var dayExpression = Build(memberExpression,
                                   Expression.Constant(dateValue.Day), "Day", "Equals", typeof(Int32));
                    var monthExpression = Build(memberExpression,
                        Expression.Constant(dateValue.Month), "Month", "Equals", typeof(Int32));
                    var yearExpression = Build(memberExpression,
                        Expression.Constant(dateValue.Year), "Year", "Equals", typeof(Int32));

                    var andExpression = Expression.AndAlso(dayExpression, monthExpression);

                    expression = Expression.AndAlso(andExpression, yearExpression);

                    expected = "DAY(SomeDate) = {0} AND MONTH(SomeDate) = {1} AND YEAR(SomeDate) = {2}"
                        .FormatString(dateValue.Day.ToString(), dateValue.Month.ToString(), dateValue.Year.ToString());

                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(DateClass));

                    Asserter.AssertEquality(new QueryContainer(expected), actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleDateTimeWithPrefix()
        {
            Expression expression = null;
            string expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var dateClass = ObjectCreator.CreateNew<DateClass>();

                    var parameterExpression = Expression.Parameter(typeof(DateClass), "d");
                    var propertyInfo = typeof(DateClass).GetProperty("SomeDateTime");
                    var memberExpression = Expression.MakeMemberAccess(parameterExpression, propertyInfo);
                    var dateValue = dateClass.SomeDateTime;

                    var dayExpression = Build(memberExpression,
                                   Expression.Constant(dateValue.Day), "Day", "Equals", typeof(Int32));
                    var monthExpression = Build(memberExpression,
                        Expression.Constant(dateValue.Month), "Month", "Equals", typeof(Int32));
                    var yearExpression = Build(memberExpression,
                        Expression.Constant(dateValue.Year), "Year", "Equals", typeof(Int32));

                    var andExpression = Expression.AndAlso(dayExpression, monthExpression);

                    expression = Expression.AndAlso(andExpression, yearExpression);

                    expected = "DAY(t1.t1_SomeDate) = {0} AND MONTH(t1.t1_SomeDate) = {1} AND YEAR(t1.t1_SomeDate) = {2}"
                        .FormatString(dateValue.Day.ToString(), dateValue.Month.ToString(), dateValue.Year.ToString());

                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(DateClass), tableAlias:"t1", fieldPrefix:"t1_");

                    Asserter.AssertEquality(new QueryContainer(expected), actual, new[] { "Parameters", "OrderByClause" });
                });
        }

        [TestMethod]
        public void WillHandleDateTimeWithParameter()
        {
            Expression<Func<DateClass, bool>> expression = null;
            string expected = null;
            DateClass dateClass = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    dateClass = ObjectCreator.CreateNew<DateClass>();

                    var dateValue = dateClass.SomeDateTime;

                    expression = d => d.Name == dateClass.Name && d.SomeDateTime.Day.Equals(dateValue.Day)
                                      && d.SomeDateTime.Month.Equals(dateValue.Month) &&
                                      d.SomeDateTime.Year.Equals(dateValue.Year);

                    expected = "SomeName = @someName AND DAY(SomeDate) = {0} AND MONTH(SomeDate) = {1} AND YEAR(SomeDate) = {2}"
                        .FormatString(dateValue.Day.ToString(), dateValue.Month.ToString(), dateValue.Year.ToString());

                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildContainer(expression, typeof(DateClass));
                    Expression<Action<SqlParameter, SqlParameter>> compareExpression =
                        (e, a) => CompareParameters(e, a);

                    Asserter.AssertEquality(new QueryContainer(expected), actual, propertiesToIgnore: new[] { "Parameters", "OrderByClause" });

                    Asserter.AssertEquality(new List<SqlParameter>
                    {
                        new SqlParameter("@someName", SqlDbType.NVarChar)
                        {
                            Value = dateClass.Name
                        }
                    }, actual.Parameters
                            .Select(p => (SqlParameter)p), 
                    additionalParameters: new Dictionary<string, object>
                    {
                        {
                            Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate,
                            compareExpression
                        }
                    });
                });
        }

        private Expression Build(MemberExpression memberExpression, ConstantExpression constantExpression,
           string propertyName, string methodName, Type methodType)
        {
            var property = typeof(DateTime).GetProperty(propertyName);
            var membExpression = Expression.MakeMemberAccess(memberExpression, property);
            var methodInfo = methodType.GetMethod(methodName, new[] { methodType });

            var expression = Expression.Call(membExpression, methodInfo, constantExpression);

            return expression;
        }

        private class DateClass
        {
            [FieldMetadata("SomeName", SqlDbType.NVarChar, "@someName")]
            public string Name { get; set; }

            [FieldMetadata("SomeDate", SqlDbType.NVarChar, "@someDate")]
            public DateTime SomeDateTime { get; set; }
        }

        private class DummyClass
        {
            public int SomeProperty { get; set; }
        }

        private void CompareParameters(SqlParameter expected, SqlParameter actual)
        {
            Asserter.AssertEquality(expected.ParameterName, actual.ParameterName);
            Asserter.AssertEquality(expected.SqlDbType, actual.SqlDbType);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
