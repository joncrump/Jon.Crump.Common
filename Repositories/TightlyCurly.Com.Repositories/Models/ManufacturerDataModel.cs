using System;
using System.Collections.Generic;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Manufacturers)]
    public class ManufacturerDataModel : ValueFactoryModelBase, IManufacturer
    {
        private IEnumerable<ICompany> _companies;
        private IEnumerable<ILocation> _locations; 

        [FieldMetadata(Columns.ManufacturerId, SqlDbType.UniqueIdentifier, Parameters.ManufacturerId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }
        
        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }

        public IEnumerable<ICompany> Companies
        {
            get
            {
                if (_companies.IsNull())
                {
                    _companies = GetOrLoadLazyValue(_companies, LoaderKeys.GetCompaniesByManufacturerId);
                }

                return _companies;
            }
            set { _companies = value; }
        }
        
        public IEnumerable<ILocation> Locations
        {
            get
            {
                if (_locations.IsNull())
                {
                    _locations = GetOrLoadLazyValue(_locations, LoaderKeys.GetLocationsByManufacturerId);
                }

                return _locations;
            }
            set { _locations = value; }
        }
    }
}
