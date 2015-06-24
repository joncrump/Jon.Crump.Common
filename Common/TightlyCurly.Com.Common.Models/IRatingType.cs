namespace TightlyCurly.Com.Common.Models
{
    public interface IRatingType : IModel
    {
		string Type { get; set; }
        string Text { get; set; }
        ILocale Locale { get; set; }
    }
}
