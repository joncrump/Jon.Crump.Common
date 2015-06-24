using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface IMenuItemText : IModel
    {
        Guid LocaleId { get; set; }
        string Text { get; set; }
        ILocale Locale { get; set; }
        string Description { get; set; }
    }
}
