namespace TightlyCurly.Com.Common.Models
{
    public interface IProductRevisionHistory : IModel
    {
        IProductVariant PreviousVariant { get; set; }
        IProductVariant CurrentVariant { get; set; }
        string Comments { get; set; }
        string Notes { get; set; }
    }
}
