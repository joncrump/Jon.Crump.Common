using System.Collections.Generic;
using System.Data;
using System.Linq;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public class QueryInfo
    {
        public string Query { get; set; }
        public IEnumerable<IDbDataParameter> Parameters { get; set; }
        public IEnumerable<TableObjectMapping> TableObjectMappings { get; set; } 

        public QueryInfo()
        {
            Parameters = new List<IDbDataParameter>();
            TableObjectMappings = new List<TableObjectMapping>();
        }

        public QueryInfo(string query) : this(query, (TableObjectMapping)null, null)
        {
        }

        public QueryInfo(string query, TableObjectMapping tableObjectMapping = null, IEnumerable<IDbDataParameter> parameters = null)
            : this(query, new[] { tableObjectMapping }, parameters)
        {
        }

        public QueryInfo(string query, IEnumerable<TableObjectMapping> tableObjectMappings = null, IEnumerable<IDbDataParameter> parameters = null)
        {
            Query = query;
            TableObjectMappings = tableObjectMappings.SafeWhere(t => t.IsNotNull()).ToList();
            Parameters = parameters;
        }
    }
}
