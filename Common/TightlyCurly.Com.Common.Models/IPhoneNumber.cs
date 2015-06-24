namespace TightlyCurly.Com.Common.Models
{
    public interface IPhoneNumber : IModel
    {
        string CountryCode { get; set; }
        string AreaCode { get; set; }
        string Prefix { get; set; }
        string Suffix { get; set; }
        string Extension { get; set; }
    }
}
