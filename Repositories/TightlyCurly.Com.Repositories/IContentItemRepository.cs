using System.Collections.Generic;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories
{
    public interface IContentItemRepository
    {
        IEnumerable<IContentItem> GetAllContentItems();
        IContentItem GetContentItemById(int id, ILocale locale);
        IContentItem SaveContentItem(IContentItem contentItem);
        void DeleteContentItem(IContentItem contentItem);
    }
}
