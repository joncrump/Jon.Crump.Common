using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface IAddress : IModel
    {
        string Line1 { get; set; }
        string Line2 { get; set; }
        string City { get; set; }
        Guid StateProvinceId { get; set; }
        IStateProvince StateProvince { get; set; }
        string PostalCode { get; set; }
    }
}
