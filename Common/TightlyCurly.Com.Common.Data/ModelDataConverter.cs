using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public class ModelDataConverter : IModelDataConverter
    {
        private DataTable _dataTable;
        private IDictionary<string, string> _columnMappings;

        public IDatatableObjectMapping ConvertToDataTable<TModel>(IEnumerable<TModel> models)
        {
            Guard.EnsureIsNotNullOrEmpty("models", models);

            BuildDataTable(models);

            return new DatatableObjectMapping(_dataTable, _columnMappings);
        }

        private void BuildDataTable<TModel>(IEnumerable<TModel> models)
        {
            var type = typeof(TModel);
            var tableAttribute = (TableAttribute)type.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();

            _dataTable = tableAttribute.IsNotNull() 
                ? BuildTableBasedOnMetadata(models, tableAttribute) 
                : BuildTableBasedOnModels(models);
        }

        private DataTable BuildTableBasedOnModels<TModel>(IEnumerable<TModel> models)
        {
            var table = new DataTable();

            BuildColumnsBasedOnFieldAttributesInternal<TModel>(table);

            foreach (var model in models)
            {
                var values = model
                    .ToDictionary()
                    .Values
                    .ToArray();

                table.Rows.Add(values);
            }

            return table;
        }

        private DataTable BuildTableBasedOnMetadata<TModel>(IEnumerable<TModel> models, TableAttribute tableAttribute)
        {
            var table = new DataTable
            {
                TableName = tableAttribute.Name
            };

            BuildColumnsBasedOnFieldAttributesInternal<TModel>(table);

            foreach (var model in models)
            {
                var values = model
                    .ToDictionary(new[] {typeof(FieldMetadataAttribute)})
                    .Values
                    .ToArray();

                table.Rows.Add(values);
            }

            return table;
        }

        private void BuildColumnsBasedOnPropertiesInternal<TModel>(DataTable table)
        {
            _columnMappings = new Dictionary<string, string>();

            foreach (var property in typeof(TModel).GetProperties())
            {
                Type type;

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = property.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    type = property.PropertyType;
                }

                table.Columns.Add(new DataColumn(property.Name, type));

                _columnMappings.Add(property.Name, property.Name);
            }
        }

        private void BuildColumnsBasedOnFieldAttributesInternal<TModel>(DataTable table)
        {
            _columnMappings = new Dictionary<string, string>();
            var columns = new List<Tuple<string, int, bool, Type>>();

            foreach (var property in typeof(TModel).GetProperties())
            {
                var attribute = (FieldMetadataAttribute) property.GetCustomAttributes(typeof(FieldMetadataAttribute), true).FirstOrDefault();

                if (attribute.IsNull())
                {
                    continue;
                }

                Type type;

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    type = property.PropertyType.GetGenericArguments()[0];
                }
                else
                {
                    type = property.PropertyType;
                }

                columns.Add(new Tuple<string, int, bool, Type>(attribute.FieldName, attribute.Order.HasValue ? attribute.Order.Value : 0, 
                    attribute.AllowDbNull, type));
            }

            foreach (var column in columns.OrderBy(c => c.Item2))
            {
                table.Columns.Add(new DataColumn(column.Item1, column.Item4)
                {
                    AllowDBNull = column.Item3
                });

                _columnMappings.Add(column.Item1, column.Item1);
            }
        }
    }
}
