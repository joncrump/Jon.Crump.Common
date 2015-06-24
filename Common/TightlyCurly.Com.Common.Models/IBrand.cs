using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IBrand : IModel
    {
        string BrandName { get; set; }
        Guid? ManufacturerId { get; set; }
        IManufacturer Manufacturer { get; set; }
        IEnumerable<ICopyright> Copyrights { get; set; }
        IEnumerable<ITrademark> Trademarks { get; set; }
    }
}
