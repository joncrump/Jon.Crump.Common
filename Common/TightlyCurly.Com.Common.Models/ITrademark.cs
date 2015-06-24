using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ITrademark : IModel
    {
        string Trademark { get; set; }
        bool? IsActive { get; set; }
        DateTime? RegisteredDate { get; set; }
        DateTime? ExpirationDate { get; set; }
        IEnumerable<ICompany> Companies { get; set; }
        IEnumerable<ICountry> Countries { get; set; }
    }
}
