using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IManufacturer : IModel
    {
        string Name { get; set; }
        IEnumerable<ICompany> Companies { get; set; }
        IEnumerable<ILocation> Locations { get; set; } 
    }
}
