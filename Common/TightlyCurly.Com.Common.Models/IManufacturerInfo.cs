using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IManufacturerInfo : IModel
    {
        Guid ManufacturerId { get; set; }
        string Name { get; set; }
        IEnumerable<ICompany> ManufacturedBy { get; set; }
        IEnumerable<ILocation> ManufacturedAt { get; set; }
        IEnumerable<IMarket> ManufacturedForMarkets { get; set; }
    }
}
