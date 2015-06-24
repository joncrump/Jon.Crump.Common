using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IProductVariant : IModel
    {
        IProduct Product { get; set; }
        string Name { get; set; }
        IEnumerable<IManufacturerInfo> Manufacturers { get; set; }
        IEnumerable<IBrand> Brands { get; set; }
        IEnumerable<IProductVariantSkuInfo> Skus { get; set; }
        IEnumerable<IMarket> Markets { get; set; }
        IEnumerable<IIngredientInfo> Ingredients { get; set; }
        IEnumerable<IProductRevisionHistory> RevisionHistory { get; set; }
        IEnumerable<ILocation> LocationsSoldAt { get; set; }
        IEnumerable<IProductCategory> ProductCategories { get; set; }
        IEnumerable<IProductVariantReview> ProductReviews { get; set; }
        IEnumerable<IProductSafetyInfo> ProductSafetyInfos { get; set; } 
        IEnumerable<IProductType> ProductTypes { get; set; }
        IEnumerable<IImageUri> ProductVariantImageUris { get; set; }
        IEnumerable<ICopyright> Copyrights { get; set; }
        IEnumerable<ITrademark> Trademarks { get; set; }
        IEnumerable<IPatent> Patents { get; set; }
        
        IEnumerable<IProductVariantNote> Notes { get; set; }
        IEnumerable<IProductVariantDescription> Descriptions { get; set; }
        IEnumerable<IProductVariantMetadata> Metadata { get; set; } 
    }
}
