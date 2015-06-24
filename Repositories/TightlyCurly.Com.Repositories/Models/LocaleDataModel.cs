using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Locales)]
    public class LocaleDataModel : ILocale
    {
        [FieldMetadata(Columns.LocaleId, SqlDbType.UniqueIdentifier, Parameters.LocaleId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.DateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.DateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Lcid, SqlDbType.Int, Parameters.Lcid)]
        public int Lcid { get; set; }

        [FieldMetadata(Columns.Code, SqlDbType.NVarChar, Parameters.Code)]
        public string Code { get; set; }

        [FieldMetadata(Columns.LocaleName, SqlDbType.NVarChar, Parameters.LocaleName)]
        public string LocaleName { get; set; }
    }
}
