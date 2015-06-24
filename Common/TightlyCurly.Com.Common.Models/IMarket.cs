using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMarket : IModel
    {
        string Name { get; set; }
        IEnumerable<ICountry> Countries { get; set; }
        IEnumerable<ICoordinates> Vertices { get; set; } 
    }
}
