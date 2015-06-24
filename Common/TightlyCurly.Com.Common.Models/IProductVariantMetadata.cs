namespace TightlyCurly.Com.Common.Models
{
    public interface IProductVariantMetadata : IModel
    {
        string Key { get; set; }
        string Value { get; set; }
        IUser EnteredBy { get; set; }
    }
}
