using System;
using System.Data;
using System.Linq;
using System.Reflection;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public class ReflectionBasedDataReaderBuilder : ValueFactoryBuilderBase, IDataReaderBuilder
    {
        public ReflectionBasedDataReaderBuilder(IValueFactory valueFactory)
            : base(valueFactory)
        {
        }

        public TItem Build<TItem>(IDataReader dataSource, string prefix = null, bool loadValueFactories = true) 
            where TItem : class, new()
        {
            Guard.EnsureIsNotNull("dataSource", dataSource);

            var itemType = typeof(TItem);

            var fieldProperties = itemType
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FieldMetadataAttribute), true).IsNotNullOrEmpty());

            var instance = (TItem)Activator.CreateInstance(itemType);

            foreach (var fieldProperty in fieldProperties)
            {
                var fieldAttribute = fieldProperty.GetCustomAttribute<FieldMetadataAttribute>();
                var columnName = fieldAttribute.FieldName;

                GetFieldValue(fieldProperty, instance, columnName, dataSource, prefix);
            }

            if (loadValueFactories)
            {
                HydrateValueFactories(instance);
            }

            return instance;
        }

        private void HydrateValueFactories<TItem>(TItem instance)
        {
            if (!(instance is ValueFactoryModelBase))
            {
                return;
            }

            var valueFactoryAttributes = typeof(TItem)
                .GetProperties()
                .Select(p => (ValueFactoryAttribute)p.GetCustomAttributes(typeof(ValueFactoryAttribute)).FirstOrDefault())
                .Where(a => a.IsNotNull())
                .ToList();

            foreach (var valueFactoryAttribute in valueFactoryAttributes)
            {
                AddValueFactory(instance as ValueFactoryModelBase, valueFactoryAttribute.ValueFactoryName, new ParameterInfo());
            }
        }

        private void GetFieldValue(PropertyInfo fieldProperty, object instance, string fieldName, 
            IDataReader dataSource, string prefix)
        {
            var propertyType = fieldProperty.PropertyType;
            var value = dataSource.Get(propertyType, "{0}{1}".FormatString( 
                prefix.IsNullOrEmpty() ? String.Empty : prefix, fieldName));
            var convertedValue = Convert.ChangeType(value, propertyType);

            fieldProperty.SetValue(instance, convertedValue);
        }
    }
}