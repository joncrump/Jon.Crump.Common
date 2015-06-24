using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.ModelDataConverterTests
{
    [TestClass]
    public class TheConvertToDataTableMethod : MsTestMoqTestBase<ModelDataConverter>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfModelsAreNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.ConvertToDataTable<TestData>(null))
                    .AndVerifyHasParameter("models");
            });
        }

        [TestMethod]
        public void WillThrowArgumentEmptyExceptionIfModelsAreEmpty()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentEmptyException>(
                        () => ItemUnderTest.ConvertToDataTable(Enumerable.Empty<IModel>()))
                    .AndVerifyHasParameter("models");
            });
        }

        [TestMethod]
        public void WillConvertModelsToDataTable()
        {
            DatatableObjectMapping expected = null;
            IEnumerable<TestData> models = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    models = CreateEnumerableOfItems(() => ObjectCreator.CreateNew<TestData>(new Dictionary<string, object>
                    {
                        {"Baz", null}
                    }));

                    expected = new DatatableObjectMapping(BuildDataTable(models), new Dictionary<string, string>
                    {
                        {"Id", "Id"},
                        {"DateCreated", "DateCreated"},
                        {"DateLastUpdated", "DateLastUpdated"},
                        {"Foo", "Foo"},
                        {"Bar", "Bar"},
                        {"Baz", "Baz"}
                    });
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.ConvertToDataTable(models);
                    Expression<Action<KeyValuePair<string, string>, KeyValuePair<string, string>>> expression = 
                        (e, a) => CompareKeyValuePairs(e, a);

                    Assert.AreEqual(expected.DataTable.Columns.Count, actual.DataTable.Columns.Count);
                    
                    for (var index = 0; index < expected.DataTable.Columns.Count; index++)
                    {
                        Asserter.AssertEquality(expected.DataTable.Columns[index].ColumnName, actual.DataTable.Columns[index].ColumnName);
                        Assert.AreEqual(expected.DataTable.Columns[index].DataType, actual.DataTable.Columns[index].DataType);
                    }

                    for (var index = 0; index < expected.DataTable.Rows.Count; index++)
                    {
                        Assert.AreEqual(expected.DataTable.Rows[index].ItemArray.Length, actual.DataTable.Rows[index].ItemArray.Length);
                        for (var field = 0; field < expected.DataTable.Rows[index].ItemArray.Length; field++)
                        {
                            Assert.AreEqual(expected.DataTable.Rows[index].ItemArray[field], actual.DataTable.Rows[index].ItemArray[field]);
                        }
                    }

                    Asserter.AssertEquality(expected.ColumnMappings, actual.ColumnMappings, additionalParameters:
                        new Dictionary<string, object>
                        {
                            {Com.Tests.Common.Constants.ParameterNames.ComparisonDelegate, expression}
                        });
                });
        }

        private void CompareKeyValuePairs(KeyValuePair<string, string> expected, 
            KeyValuePair<string, string> actual)
        {
            Asserter.AssertEquality(expected.Key, actual.Value);
            Asserter.AssertEquality(expected.Value, actual.Value);
        }

        private DataTable BuildDataTable(IEnumerable<TestData> models)
        {
            var type = typeof (TestData);

            var table = new DataTable();

            var tableAttribute = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();

            table.TableName = tableAttribute.Name;

            BuildColumns(table);

            foreach (var model in models)
            {
                var dictionary = model.ToDictionary(new [] {typeof(FieldMetadataAttribute)});

                table.Rows.Add(dictionary.Values.ToArray());
            }

           return table;
        }

        private void BuildColumns(DataTable table)
        {
            foreach (var property in typeof (TestData).GetProperties())
            {
                var attribute = (FieldMetadataAttribute)
                                           property.GetCustomAttributes(typeof(FieldMetadataAttribute), true).FirstOrDefault();

                if (attribute.IsNull())
                {
                    continue;
                }

                Type type;

                // We need to check whether the property is NULLABLE
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                    type = property.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    type = property.PropertyType;
                }

                table.Columns.Add(attribute.FieldName, type);
            }
        }

        [Table("MyTable")]
        public class TestData : IModel
        {
            [FieldMetadata("Id", SqlDbType.UniqueIdentifier, "@id")]
            public Guid Id { get; set; }

            public DateTime EnteredDate { get; set; }
            public DateTime UpdatedDate { get; set; }

            [FieldMetadata("DateCreated", SqlDbType.SmallDateTime, "@dateCreated")]
            public DateTime DateCreated { get; set; }

            [FieldMetadata("DateLastUpdated", SqlDbType.SmallDateTime, "@dateLastUpdated")]
            public DateTime DateLastUpdated { get; set; }

            [FieldMetadata("Foo", SqlDbType.NVarChar, "@foo")]
            public string Foo { get; set; }

            [FieldMetadata("Bar", SqlDbType.Int, "@bar")]
            public int Bar { get; set; }

            [FieldMetadata("Baz", SqlDbType.Int, "@baz", allowDbNull:true)]
            public int? Baz { get; set; }
        }
    }
}
