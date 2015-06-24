namespace TightlyCurly.Com.Common.Models
{
    public interface IProductVariantDescription : IModel
    {
        string Description { get; set; }
        IUser EnteredBy { get; set; }
    }
}
