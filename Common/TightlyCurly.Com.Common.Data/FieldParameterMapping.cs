using System.Data;

namespace TightlyCurly.Com.Common.Data
{
    public class FieldParameterMapping
    {
        public FieldParameterMapping()
        {
        }

        public FieldParameterMapping(string fieldName, string parameterName, SqlDbType dbType, object value = null, 
            bool isIdentity = false, string prefix = null)
        {
            FieldName = fieldName;
            ParameterName = parameterName;
            DbType = dbType;
            Value = value;
            IsIdentity = isIdentity;
            Prefix = prefix;
        }

        public string FieldName { get; set; }
        public string ParameterName { get; set; }
        public SqlDbType DbType { get; set; }
        public object Value { get; set; }
        public bool IsIdentity { get; set; }
        public string Prefix { get; set; }
    }
}
