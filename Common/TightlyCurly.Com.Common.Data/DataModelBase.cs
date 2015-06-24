using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;

namespace TightlyCurly.Com.Common.Data
{
    public abstract class DataModelBase
    {
        [FieldMetadata("DateCreated", SqlDbType.SmallDateTime, "@dateCreated")]
        public DateTime DateCreated { get; set; }

        [FieldMetadata("DateLastUpdated", SqlDbType.SmallDateTime, "@DateLastUpdated")]
        public DateTime DateLastUpdated { get; set; }
    }
}
