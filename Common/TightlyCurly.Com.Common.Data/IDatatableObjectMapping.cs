using System.Collections.Generic;
using System.Data;

namespace TightlyCurly.Com.Common.Data
{
    public interface IDatatableObjectMapping
    {
        DataTable DataTable { get; set; }
        IDictionary<string, string> ColumnMappings { get; set; }
    }
}
