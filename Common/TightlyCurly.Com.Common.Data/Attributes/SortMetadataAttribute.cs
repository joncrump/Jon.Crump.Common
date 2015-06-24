using System;

namespace TightlyCurly.Com.Common.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SortMetadataAttribute : Attribute
    {
        public SortMetadataAttribute(string sortColumn, string field, bool isPrimitive = false)
        {
            SortColumn = sortColumn;
            Field = field;
            IsPrimitive = isPrimitive;
        }
        
        public string SortColumn { get; private set; }
        public string Field { get; private set; }
        public bool IsPrimitive { get; private set; }
    }
}
