using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IContentArticle : IMetaItem, IModel
    {
        // Contains locale, description, text, etc.
        IEnumerable<IContentArticleData> Data { get; set; }
        bool IsActive { get; set; }
        Guid ContentItemId { get; set; }
        IEnumerable<IContentArticleNote> Notes { get; set; }
        // Contains article revisions, such as edited by, text, etc.
        IEnumerable<IContentArticleRevision> Revisions { get; set; }
        IContentItem ContentItem { get; set; }
    }
}
