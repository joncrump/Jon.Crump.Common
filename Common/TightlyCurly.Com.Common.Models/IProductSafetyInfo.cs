namespace TightlyCurly.Com.Common.Models
{
    public interface IProductSafetyInfo : IModel
    {
        ProductSafetyInfoType ProductSafetyInfoType { get; set; }
        string Text { get; set; }
    }
}
