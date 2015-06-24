namespace TightlyCurly.Com.Common.Models
{
    public interface ILocale : IModel
    {
        int Lcid { get; set; }
        string Code { get; set; }
        string LocaleName { get; set; }
    }
}
