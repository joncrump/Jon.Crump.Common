using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Addresses)]
    public class AddressDataModel : ValueFactoryModelBase, IAddress
    {
        private ICountry _country;
        private IStateProvince _stateProvince;

        [FieldMetadata(Columns.AddressId, SqlDbType.UniqueIdentifier, Parameters.AddressId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.Line1, SqlDbType.NVarChar, Parameters.Line1)]
        public string Line1 { get; set; }

        [FieldMetadata(Columns.Line2, SqlDbType.NVarChar, Parameters.Line2)]
        public string Line2 { get; set; }

        [FieldMetadata(Columns.City, SqlDbType.NVarChar, Parameters.City)]
        public string City { get; set; }

        [FieldMetadata(Columns.StateProvinceId, SqlDbType.UniqueIdentifier, Parameters.StateProvinceId)]
        public Guid StateProvinceId { get; set; }

        [FieldMetadata(Columns.PostalCode, SqlDbType.NVarChar, Parameters.PostalCode)]
        public string PostalCode { get; set; }

        [FieldMetadata(Columns.CountryId, SqlDbType.UniqueIdentifier, Parameters.CountryId)]
        public Guid? CountryId { get; set; }

        [ValueFactory(LoaderKeys.GetCountryByAddress)]
        [Join(JoinType.Left, typeof(CountryDataModel), Columns.CountryId, Columns.CountryId)]
        public ICountry Country
        {
            get
            {
                if (_country.IsNull())
                {
                    _country = GetOrLoadLazyValue(_country, LoaderKeys.GetCountryByAddress);
                }

                return _country;
            }
            set { _country = value; }
        }

        [ValueFactory(LoaderKeys.GetStateProvinceByAddress)]
        [Join(JoinType.Left, typeof(StateProvinceDataModel), Columns.StateProvinceId, Columns.StateProvinceId)]
        public IStateProvince StateProvince
        {
            get
            {
                if (_stateProvince.IsNull())
                {
                    _stateProvince = GetOrLoadLazyValue(_stateProvince, LoaderKeys.GetStateProvinceByAddress);
                }

                return _stateProvince;
            }
            set { _stateProvince = value; }
        }
    }
}
