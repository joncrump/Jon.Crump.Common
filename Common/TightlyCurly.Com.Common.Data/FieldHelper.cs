using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public class FieldHelper : IFieldHelper
    {
        public TableObjectMapping BuildFields<TValue>(IEnumerable<string> desiredFields = null, 
            string tableName = null, TValue model = default(TValue), bool ignoreIdentity = false, 
            string alias = null, string instancePropertyName = null) 
            where TValue : class
        {
            var type = typeof(TValue);
            var fields = new Dictionary<string, FieldParameterMapping>();

            if (tableName.IsNullOrEmpty())
            {
                var tableAttribute =
                    (TableAttribute) type.GetCustomAttributes(typeof (TableAttribute), true).FirstOrDefault();

                if (tableAttribute.IsNull())
                {
                    throw new InvalidOperationException("Cannot build query.  Type {0} has no table attributes"
                        .FormatString(type));
                }

// ReSharper disable once PossibleNullReferenceException
                tableName = tableAttribute.Name;
            }

            foreach (var property in type.GetProperties())
            {
                var attribute =
                    (FieldMetadataAttribute)property.GetCustomAttributes(typeof(FieldMetadataAttribute), true).FirstOrDefault();

                if (attribute.IsNull())
                {
                    continue;
                }

// ReSharper disable once PossibleNullReferenceException
                if (ignoreIdentity && attribute.IsIdentity)
                {
                    continue;
                }

// ReSharper disable once AssignNullToNotNullAttribute
                if ((desiredFields.IsNotNullOrEmpty() && !desiredFields.Contains(property.Name)))
                {
                    continue;
                }

// ReSharper disable once PossibleNullReferenceException
                fields.Add(property.Name, new FieldParameterMapping(attribute.FieldName, attribute.ParameterName, attribute.DbType, 
                    EqualityComparer<TValue>.Default.Equals(model, default(TValue)) ? null : property.GetValue(model), attribute.IsIdentity, alias));
            }

            if (fields.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Cannot build query.  Model has no data attributes.");
            }

            return new TableObjectMapping
            {
                TableName = tableName,
                FieldMappings = fields,
                Alias =  alias,
                InstancePropertyName = instancePropertyName
            };
        }

        public IEnumerable<IDbDataParameter> ExtractParameters(TableObjectMapping tableFieldInfo, bool ignoreIdentity)
        {
            if (tableFieldInfo.IsNull() || tableFieldInfo.FieldMappings.IsNullOrEmpty())
            {
                return Enumerable.Empty<IDbDataParameter>();
            }

            return tableFieldInfo
                .FieldMappings
                .Select(f => f.Value)
                .Where(f => !ignoreIdentity || !f.IsIdentity)
                .SafeSelect(f => new SqlParameter(f.ParameterName, f.DbType)
                {
                    Value = f.Value.IsNull() ? DBNull.Value : f.Value
                })
                .ToList();
        }
    }
}
