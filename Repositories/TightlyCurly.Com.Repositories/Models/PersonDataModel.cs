using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.People)]
    public class PersonDataModel : ValueFactoryModelBase, IPerson
    {
        [FieldMetadata(Columns.PersonId, SqlDbType.UniqueIdentifier, Parameters.PersonId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Columns.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Columns.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.FirstName, SqlDbType.NVarChar, Parameters.FirstName)]
        public string FirstName { get; set; }

        [FieldMetadata(Columns.MiddleName, SqlDbType.NVarChar, Parameters.MiddleName)]
        public string MiddleName { get; set; }

        [FieldMetadata(Columns.LastName, SqlDbType.NVarChar, Parameters.LastName)]
        public string LastName { get; set; }

        [FieldMetadata(Columns.Gender, SqlDbType.NVarChar, Parameters.Gender)]
        public string Gender { get; set; }

        [FieldMetadata(Columns.Honorific, SqlDbType.NVarChar, Parameters.Honorific)]
        public string Honorific { get; set; }

        [FieldMetadata(Columns.Suffix, SqlDbType.NVarChar, Parameters.Suffix)]
        public string Suffix { get; set; }

        public ILocale PreferredLocale { get; set; }
        public IEnumerable<ISocialMediaInfo> SocialMediaInfo { get; set; }
        public IEnumerable<IContactInfo> ContactInfo { get; set; }
        public IEnumerable<IPersonRelationship> Relationships { get; set; }
    }
}
