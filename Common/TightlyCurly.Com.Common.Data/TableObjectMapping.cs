using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Data
{
    public class TableObjectMapping
    {
        public TableObjectMapping()
        {
            FieldMappings = new Dictionary<string, FieldParameterMapping>();
            Joins = new List<Join>();
        }

        public string InstancePropertyName { get; set; }
        public string TableName { get; set; }
        public string Alias { get; set; }
        public IDictionary<string, FieldParameterMapping> FieldMappings { get; set; }
        public IList<Join> Joins { get; set; }
        public string Prefix { get; set; }
        public string ParentKey { get; set; }
        public string ChildKey { get; set; }
        public Type JoinType { get; set; }
    }
}
