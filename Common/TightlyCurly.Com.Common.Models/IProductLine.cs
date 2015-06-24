using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IProductLine : IModel
    {
        string Name { get; set; }
        IEnumerable<ICompany> Companies { get; set; }
        IEnumerable<ICopyright> Copyrights { get; set; }
        IEnumerable<IPatent> Patents { get; set; }
        IEnumerable<IProduct> Products { get; set; }
        IEnumerable<ITrademark> Trademarks { get; set; } 
    }
}
