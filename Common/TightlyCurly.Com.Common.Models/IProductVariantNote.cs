namespace TightlyCurly.Com.Common.Models
{
    public interface IProductVariantNote : IModel
    {
        string Text { get; set; }
        IUser EnteredBy { get; set; }
    }
}
