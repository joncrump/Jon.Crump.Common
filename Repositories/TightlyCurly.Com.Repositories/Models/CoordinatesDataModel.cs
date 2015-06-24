using System;
using System.Data;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Constants;

namespace TightlyCurly.Com.Repositories.Models
{
    [Table(Tables.Coordinates)]
    public class CoordinatesDataModel : ICoordinates
    {
        [FieldMetadata(Columns.CoordinateId, SqlDbType.UniqueIdentifier, Parameters.CoordinateId)]
        public Guid Id { get; set; }
        
        [FieldMetadata(Columns.EnteredDate, SqlDbType.SmallDateTime, Parameters.EnteredDate)]
        public DateTime EnteredDate { get; set; }

        [FieldMetadata(Columns.UpdatedDate, SqlDbType.SmallDateTime, Parameters.UpdatedDate)]
        public DateTime UpdatedDate { get; set; }

        [FieldMetadata(Columns.ObjectType, SqlDbType.NVarChar, Parameters.ObjectType)]
        public string ObjectType { get; set; }

        [FieldMetadata(Columns.ObjectId, SqlDbType.UniqueIdentifier, Parameters.ObjectId)]
        public Guid ObjectId { get; set; }

        [FieldMetadata(Columns.Latitude, SqlDbType.NVarChar, Parameters.Latitude)]
        public string Latitude { get; set; }

        [FieldMetadata(Columns.Longitude, SqlDbType.NVarChar, Parameters.Longitude)]
        public string Longitude { get; set; }
    }
}
