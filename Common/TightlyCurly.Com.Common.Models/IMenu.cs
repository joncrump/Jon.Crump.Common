using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMenu : IModel
    {
        IContentItem ContentItem { get; set; }
        string Name { get; set; }
        IEnumerable<IMenuItem> MenuItems { get; set; }
        IEnumerable<IMenuText> MenuTexts { get; set; } 
    }
}
