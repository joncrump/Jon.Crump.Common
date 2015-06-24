using System.Collections.Generic;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories
{
    public interface IContentArticleRepository
    {
        IEnumerable<IContentArticle> GetContentArticlesByContentItem(IContentItem contentItem);
        IContentArticle GetContentArticleById(int id, ILocale locale);
        IContentArticle SaveContentArticle(IContentArticle contentArticle);
        void DeleteContentArticle(IContentArticle contentArticle);
    }
}
