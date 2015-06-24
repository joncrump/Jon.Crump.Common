namespace TightlyCurly.Com.Common.Models
{
    public interface IAside : IModel, IOrderable
    {
        string Text { get; set; }
        bool? IsActive { get; set; }
    }
}
