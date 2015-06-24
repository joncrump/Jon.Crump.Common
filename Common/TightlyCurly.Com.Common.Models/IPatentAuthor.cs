using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IPatentAuthor : IPerson
    {
        IEnumerable<IPatent> Patents { get; set; } 
    }
}
