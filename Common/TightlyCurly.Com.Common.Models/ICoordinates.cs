using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface ICoordinates : IModel
    {
        string ObjectType { get; set; }
        Guid ObjectId { get; set; }
        string Latitude { get; set; }
        string Longitude { get; set; }
    }
}
