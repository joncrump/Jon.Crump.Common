using TightlyCurly.Com.Common.Model.Enums;

namespace TightlyCurly.Com.Common.Models
{
    public interface ITextEntry
    {
        ILocale Locale { get; set; }
        string Text { get; set; }
        ContentType? ContentType { get; set; }
    }
}
