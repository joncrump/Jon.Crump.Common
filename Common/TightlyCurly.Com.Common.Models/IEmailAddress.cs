namespace TightlyCurly.Com.Common.Models
{
    public interface IEmailAddress : IModel
    {
        string Address { get; set; }
        bool? Verified { get; set; }
        bool? IsPrimary { get; set; }
        bool? IsActive { get; set; }
    }
}
