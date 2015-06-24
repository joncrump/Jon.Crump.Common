using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface ICopyright : IModel
    {
        string Copyright { get; set; }
        bool? IsActive { get; set; }
        DateTime? RegisteredDate { get; set; }
        DateTime? ExpirationDate { get; set; }
        IEnumerable<ICompany> Owners { get; set; }
        IEnumerable<ICountry> Countries { get; set; }
    }
}
