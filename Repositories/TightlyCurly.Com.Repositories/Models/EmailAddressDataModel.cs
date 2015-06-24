using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.EmailAddresses)]
    public class EmailAddressDataModel : IEmailAddress
    {
        [FieldMetadata(Columns.EmailAddressId, SqlDbType.UniqueIdentifier, Parameters.EmailAddressId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Address, SqlDbType.NVarChar, Parameters.Address)]
        public string Address { get; set; }

        [FieldMetadata(Columns.Verified, SqlDbType.Bit, Parameters.Verified)]
        public bool? Verified { get; set; }

        [FieldMetadata(Columns.IsPrimary, SqlDbType.Bit, Parameters.IsPrimary)]
        public bool? IsPrimary { get; set; }

        public bool? IsActive { get; set; }
    }
}
