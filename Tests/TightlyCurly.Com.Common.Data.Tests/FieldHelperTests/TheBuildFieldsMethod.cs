using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.FieldHelperTests
{
    [TestClass]
    public class TheBuildFieldsMethod : MsTestMoqTestBase<FieldHelper>
    {
        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfTableNameIsNullAndObjectHasNoTableAttribute()
        {
            TestRunner.ExecuteTest(() =>
            {
                // ReSharper disable once RedundantArgumentDefaultValue
                Asserter
                    .AssertExceptionIsThrown<InvalidOperationException>(
                        () => ItemUnderTest.BuildFields<object>(tableName: null))
                    .AndVerifyMessageContains(
                        "Cannot build query.  Type {0} has no table attributes".FormatString(typeof (object)));
            });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfTableNameIsEmptyAndObjectHasNoTableAttribute()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<InvalidOperationException>(
                        () => ItemUnderTest.BuildFields<object>(tableName: String.Empty))
                    .AndVerifyMessageContains(
                        "Cannot build query.  Type {0} has no table attributes".FormatString(typeof (object)));
            });
        }

        [TestMethod]
        public void WillReturnFieldsWithoutInstance()
        {
            TableObjectMapping expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = new TableObjectMapping
                    {
                        TableName = "dbo.TestTable", 
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                            {
                                {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, null, true)},
                                {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar)},
                                {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar)}
                            }
                    };
                }).ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildFields<TestClass>();
                    Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression = 
                        (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNull(f, null));

                    Asserter.AssertEquality(expected, actual, new[] {"FieldMappings","Joins", "JoinType"});

                    Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
                        new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                        });
                });
        }

        [TestMethod]
        public void WillReturnFieldsWithInstance()
        {
            TableObjectMapping expected = null;
            TestClass model = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    model = ObjectCreator.CreateNew<TestClass>();

                    expected = new TableObjectMapping
                    {
                        TableName = "dbo.TestTable", 
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                            {
                                {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, model.Id, true)},
                                {"Foo", new FieldParameterMapping("SomeFoo", "@fooParameter", SqlDbType.NVarChar, model.Foo)},
                                {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, model.Bar)}
                            }
                    };
                }).ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildFields(model:model);

                    Asserter.AssertEquality(expected, actual, new[] {"FieldMappings", "Joins", "JoinType"});
                    Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
                        (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNotNull(f, null));

                    Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
                        new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                        });
                });
        }

        [TestMethod]
        public void WillReturnSelectedFieldsWithInstance()
        {
            TableObjectMapping expected = null;
            TestClass model = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    model = ObjectCreator.CreateNew<TestClass>();

                    expected = new TableObjectMapping
                    {
                        TableName = "dbo.TestTable", 
                        FieldMappings = new Dictionary<string, FieldParameterMapping>
                            {
                                {"Id", new FieldParameterMapping("TestClassId", "@id", SqlDbType.Int, model.Id, true)},
                                {"Bar", new FieldParameterMapping("PioneerSquareBar", "@itsFridayLetsGoToTheBar", SqlDbType.NVarChar, model.Bar)}
                            }
                    };
                }).ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildFields(new[] {"Id", "Bar"}, model: model);
                    Expression<Action<KeyValuePair<string, FieldParameterMapping>, KeyValuePair<string, FieldParameterMapping>>> expression =
                        (e, a) => CompareFieldParameterInfos(e, a, f => Assert.IsNotNull(f, null));

                    Asserter.AssertEquality(expected, actual, new[] {"FieldMappings", "Joins", "JoinType"});

                    Asserter.AssertEquality(expected.FieldMappings, actual.FieldMappings, additionalParameters:
                        new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                        });
                });
        }

        [TestMethod]
        public void WillSetTableName()
        {
            string tableName = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    tableName = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildFields<TestClass>(tableName: tableName);

                    Asserter.AssertEquality(tableName, actual.TableName);
                });
        }

        [TestMethod]
        public void WillSetAlias()
        {
            string alias = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    alias = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.BuildFields<TestClass>(alias: alias);

                    Asserter.AssertEquality(alias, actual.Alias);
                });
        }
        
        private void CompareFieldParameterInfos(KeyValuePair<string, FieldParameterMapping> expected, 
            KeyValuePair<string, FieldParameterMapping> actual, Action<object> valueAsserter)
        {
            Asserter.AssertEquality(expected.Key, actual.Key);
            Asserter.AssertEquality(expected.Value, actual.Value, new[] {"DbType", "Value"});
            Asserter.AssertEquality(expected.Value.DbType, actual.Value.DbType);

            valueAsserter(actual.Value.Value);
        }
    }
}