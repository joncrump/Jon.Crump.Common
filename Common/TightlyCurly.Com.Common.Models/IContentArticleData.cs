using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface IContentArticleData : IModel
    {
        string Description { get; set; }
        string Text { get; set; }
        ILocale Locale { get; set; }
        Guid LocaleId { get; set; }
    }
}
