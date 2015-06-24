using System;
using System.Data;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Locations)]
    public class LocationDataModel : ValueFactoryModelBase, ILocation
    {
        private IAddress _address;
        private ICoordinates _coordinates;

        [FieldMetadata(Columns.LocationId, SqlDbType.UniqueIdentifier, Parameters.LocationId)]
        public Guid Id { get; set; }

        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.Name, SqlDbType.NVarChar, Parameters.Name)]
        public string Name { get; set; }
        
        public IAddress Address
        {
            get
            {
                if (_address.IsNull())
                {
                    _address = GetOrLoadLazyValue(_address, LoaderKeys.GetAddressByLocationId);
                }

                return _address;
            }
            set { _address = value; }
        }

        public ICoordinates Coordinates
        {
            get
            {
                if (_coordinates.IsNull())
                {
                    _coordinates = GetOrLoadLazyValue(_coordinates, LoaderKeys.GetCoordinatesByLocationId);
                }

                return _coordinates;
            }
            set { _coordinates = value; }
        }
    }
}
