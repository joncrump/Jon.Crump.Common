using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IContentItem : IModel, IMetaItem
    {
        string Title { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
        Uri Uri { get; set; }
        string Name { get; set; }
        IEnumerable<IContentArticle> Articles { get; set; }
    }
}
