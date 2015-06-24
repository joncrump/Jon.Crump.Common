using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IPatent : IModel
    {
        string PatentNumber { get; set; }
        DateTime? RegisteredDate { get; set; }
        DateTime? ExpirationDate { get; set; }
        bool? IsActive { get; set; }
        bool? IsPending { get; set; }
        IEnumerable<IPatentAuthor> Authors { get; set; }
        IEnumerable<ICompany> Owners { get; set; }
        IEnumerable<ICountry> Countries { get; set; } 
    }
}
