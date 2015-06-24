using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Asides)]
    public class AsideDataModel : IAside
    {
        [FieldMetadata(Columns.AsideId, SqlDbType.UniqueIdentifier, Parameters.AsideId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.Order, SqlDbType.Int, Parameters.Order, allowDbNull: true)]
        public int? Order { get; set; }

        [FieldMetadata(Columns.Text, SqlDbType.NVarChar, Parameters.Text)]
        public string Text { get; set; }

        [FieldMetadata(Columns.IsActive, SqlDbType.Bit, Parameters.IsActive, allowDbNull: true)]
        public bool? IsActive { get; set; }
    }
}
