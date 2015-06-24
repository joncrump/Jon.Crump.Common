using System.Collections.Generic;
using System.Data;

namespace TightlyCurly.Com.Common.Data
{
    public class QueryContainer
    {
        public QueryContainer(string whereClause, IEnumerable<IDbDataParameter> parameters = null, string orderByClause = null)
        {
            WhereClause = whereClause;
            OrderByClause = orderByClause;
            Parameters = parameters;
        }

        public string WhereClause { get; private set; }
        public string OrderByClause { get; private set; }
        public IEnumerable<IDbDataParameter> Parameters { get; private set; } 
    }
}
