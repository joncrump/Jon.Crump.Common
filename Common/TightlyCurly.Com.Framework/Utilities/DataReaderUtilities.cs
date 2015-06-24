using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class DataReaderUtilities
    {
        public static bool ColumnExists(this IDataReader reader, string columnName)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return false;
            }

            reader.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (reader.GetSchemaTable().DefaultView.Count > 0);
        }
    }
}
