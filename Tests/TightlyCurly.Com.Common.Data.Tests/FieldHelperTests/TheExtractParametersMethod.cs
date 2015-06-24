using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.FieldHelperTests
{
    [TestClass]
    public class TheExtractParametersMethod : MsTestMoqTestBase<FieldHelper>
    {
        [TestMethod]
        public void WillReturnEmptyEnumerableIfFieldContainersAreNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                var actual = ItemUnderTest.ExtractParameters(null, It.IsAny<bool>());

                Assert.IsNotNull(actual);
                Assert.AreEqual(0, actual.Count());
            });
        }

        [TestMethod]
        public void WillReturnEmptyEnumerableIfFieldContainersValuesAreNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                var actual =
                    ItemUnderTest.ExtractParameters(new TableObjectMapping
                    {
                        TableName = DataGenerator.GenerateString()
                    }, It.IsAny<bool>());

                Assert.IsNotNull(actual);
                Assert.AreEqual(0, actual.Count());
            });
        }

        [TestMethod]
        public void WillReturnEmptyEnumerableIfFieldContainersValuesAreEmpty()
        {
            TestRunner.ExecuteTest(() =>
            {
                var actual =
                    ItemUnderTest.ExtractParameters(new TableObjectMapping
                    {
                        TableName = DataGenerator.GenerateString(),
                        FieldMappings = new Dictionary<string, FieldParameterMapping>()
                    }, It.IsAny<bool>());

                Assert.IsNotNull(actual);
                Assert.AreEqual(0, actual.Count());
            });
        }

        [TestMethod]
        public void WillReturnAllParametersIfIgnoreIdentityIsFalse()
        {
            IEnumerable<IDbDataParameter> expected = null;
            TableObjectMapping values = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var id = DataGenerator.GenerateInteger();
                    var barRescue = DataGenerator.GenerateDateTime();
                    var baz = DataGenerator.GenerateString();

                    values = new TableObjectMapping
                    {
                        TableName = DataGenerator.GenerateString(),
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                            {
                                {"Id", new FieldParameterMapping("TestId", "@theId", SqlDbType.Int, id, true)},
                                {"Bar", new FieldParameterMapping("TestBar", "@barRescue", SqlDbType.DateTime, barRescue)},
                                {"Baz", new FieldParameterMapping("TestBaz", "@baz", SqlDbType.NVarChar, baz)},
                                {"Bak", new FieldParameterMapping("TestBak", "@bak", SqlDbType.NVarChar)}
                            }
                    };
                        
                    expected = new List<IDbDataParameter>
                    {
                        new SqlParameter("@theId", SqlDbType.Int) {Value = id},
                        new SqlParameter("@barRescue", SqlDbType.DateTime) {Value = barRescue},
                        new SqlParameter("@baz", SqlDbType.NVarChar) {Value = baz},
                        new SqlParameter("@bak", SqlDbType.NVarChar) {Value = DBNull.Value}
                    };
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.ExtractParameters(values, false);
                    Expression<Action<IDbDataParameter, IDbDataParameter>> expression =
                        (e, a) => CompareParameters(e, a);
                    
                    Asserter.AssertEquality(expected, actual, additionalParameters:
                        new Dictionary<string, object>
                        {
                            {
                                Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                                expression
                            }
                        });
                });
        }

        [TestMethod]
        public void WillReturnAllParametersIfIgnoreIdentityIsTrue()
        {
            IEnumerable<IDbDataParameter> expected = null;
            TableObjectMapping values = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    var id = DataGenerator.GenerateInteger();
                    var barRescue = DataGenerator.GenerateDateTime();
                    var baz = DataGenerator.GenerateString();

                    values = new TableObjectMapping
                    {
                        TableName = DataGenerator.GenerateString(),
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                            {
                                {"Id", new FieldParameterMapping("TestId", "@theId", SqlDbType.Int, id, true)},
                                {"Bar", new FieldParameterMapping("TestBar", "@barRescue", SqlDbType.DateTime, barRescue)},
                                {"Baz", new FieldParameterMapping("TestBaz", "@baz", SqlDbType.NVarChar, baz)},
                                {"Bak", new FieldParameterMapping("TestBak", "@bak", SqlDbType.NVarChar)}
                            }
                    };
                        
                    expected = new List<IDbDataParameter>
                    {
                        new SqlParameter("@barRescue", SqlDbType.DateTime) {Value = barRescue},
                        new SqlParameter("@baz", SqlDbType.NVarChar) {Value = baz},
                        new SqlParameter("@bak", SqlDbType.NVarChar) {Value = DBNull.Value}
                    };
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.ExtractParameters(values, true);
                    Expression<Action<IDbDataParameter, IDbDataParameter>> expression =
                        (e, a) => CompareParameters(e, a);

                    Asserter.AssertEquality(expected, actual, additionalParameters:
                        new Dictionary<string, object>
                        {
                            {
                                Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, 
                                expression
                            }
                        });
                });
        }

        private void CompareParameters(IDbDataParameter expected, IDbDataParameter actual)
        {
            Asserter.AssertEquality(expected.ParameterName, actual.ParameterName);
            Asserter.AssertEquality(expected.DbType, actual.DbType);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}
