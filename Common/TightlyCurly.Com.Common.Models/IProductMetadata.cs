using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IProductMetadata : IModel
    {
        IEnumerable<IBrand> Brands { get; set; }
        IProduct Product { get; set; }
        Guid ProductId { get; set; }
        string Title { get; set; }
        IEnumerable<IProductLine> ProductLines { get; set; }
        IEnumerable<IImageUri> ImageUris { get; set; }
    }
}
