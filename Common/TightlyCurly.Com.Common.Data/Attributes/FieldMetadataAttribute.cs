using System;
using System.Data;

namespace TightlyCurly.Com.Common.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldMetadataAttribute : Attribute, IOrderable
    {
        public FieldMetadataAttribute(string fieldName, SqlDbType dbType = SqlDbType.Image,
            string parameterName = null, bool isIdentity = false, bool allowDbNull = false, int order = 0,
            Type mappedType = null, bool isPrimaryKey = false)
        {
            FieldName = fieldName;
            DbType = dbType;
            ParameterName = parameterName;
            IsIdentity = isIdentity;
            AllowDbNull = allowDbNull;
            Order = order;
            MappedType = mappedType;
            IsPrimaryKey = isPrimaryKey;
        }

        public string FieldName { get; private set; }
        public SqlDbType DbType { get; private set; }
        public string ParameterName { get; private set; }
        public bool IsIdentity { get; private set; }
        public bool AllowDbNull { get; private set; }
        public int? Order { get; private set; }
        public Type MappedType { get; private set; }
        public bool IsPrimaryKey { get; private set; }
    }
}