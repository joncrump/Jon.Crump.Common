using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMetaItem
    {
        string MetaDescription { get; set; }
        IEnumerable<string> MetaKeywords { get; set; }
    }
}
