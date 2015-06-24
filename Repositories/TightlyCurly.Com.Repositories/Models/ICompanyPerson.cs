using System.Collections.Generic;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories.Models
{
    public interface ICompanyPerson : IPerson
    {
        IEnumerable<ICompanyPosition> Positions { get; set; }
    }
}
