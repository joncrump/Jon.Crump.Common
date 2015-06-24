using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Countries)]
    public class CountryDataModel : ICountry
    {
        [FieldMetadata(Columns.CountryId, SqlDbType.UniqueIdentifier, Parameters.CountryId)]
        public Guid Id { get; set; }
        
        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }
    }
}
