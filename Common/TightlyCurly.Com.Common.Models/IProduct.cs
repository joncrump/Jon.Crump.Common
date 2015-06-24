using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IProduct : IModel
    {
        IEnumerable<IProductMetadata> ProductMetadata { get; set; }
        //TODO: Add Database tables.
        IEnumerable<IProductVariant> Variants { get; set; } 
    }
}
