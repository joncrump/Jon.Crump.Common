using System;
using System.Collections.Generic;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common
{
    public class ColumnInfo
    {
        public ColumnInfo() 
        {
            Values = new List<object>();
        }

        public ColumnInfo(string columnName)
        {
            ColumnName = Guard.EnsureIsNotNullOrEmpty("columnName", columnName);
        }

        public ColumnInfo(string columnName, object value) : this(columnName)
        {
            Values = new List<object> {value};
        }

        public ColumnInfo(string columName, IEnumerable<object> values) : this(columName)
        {
            Values = values.ToSafeList();
        }

        public string ColumnName { get; set; }
        public IEnumerable<object> Values { get; set; } 
    }
}
