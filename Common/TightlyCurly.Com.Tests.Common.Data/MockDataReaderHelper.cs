using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Data
{
    public class MockDataReaderHelper : IDataReaderHelper
    {
        public MockDataReader BuildMockDataReader<TModel>( 
            IEnumerable<TModel> instances, string prefix = null)
            where TModel : class, new()
        {
            var columns = new List<ColumnInfo>();

            var fieldProperties = typeof(TModel)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FieldMetadataAttribute), true)
                    .IsNotNullOrEmpty());

            foreach (var property in fieldProperties)
            {
                var attribute = (FieldMetadataAttribute)property
                    .GetCustomAttributes(typeof(FieldMetadataAttribute), true)
                    .First();
                columns.Add(new ColumnInfo("{0}{1}".FormatString(prefix.IsNullOrEmpty() ? String.Empty : prefix, attribute.FieldName), 
                    GetValuesFromProperty(instances, attribute, property)));
            }
            
            var reader = new MockDataReader(new DataContainer(instances.Count(), columns));

            reader.Read();

            return reader;
        }

        private IEnumerable<object> GetValuesFromProperty<TModel>(IEnumerable<TModel> instances, FieldMetadataAttribute attribute, PropertyInfo property)
        {
            var values = new List<object>();

            foreach (var instance in instances)
            {
                var value = property.GetValue(instance);
                
                if (typeof(Enum).IsAssignableFrom(property.PropertyType))
                {
                    var underlyingType = Enum.GetUnderlyingType(property.PropertyType);
                    value = Convert.ChangeType(value, underlyingType);
                }
                else if (attribute.MappedType.IsNotNull())
                {
                    value = Convert.ChangeType(value, attribute.MappedType);
                }

                values.Add(value);
            }

            return values;
        }
    }
}