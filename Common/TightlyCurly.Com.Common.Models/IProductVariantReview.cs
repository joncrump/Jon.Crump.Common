using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IProductVariantReview : IModel
    {
        IEnumerable<IProductVariantEditorialReview> EditorialReviews { get; set; }
        IEnumerable<IProductVariantUserReview> UserReviews { get; set; } 
        decimal OverallRating { get; set; }
        IEnumerable<string> Comments { get; set; }
        IEnumerable<string> Notes { get; set; }
    }
}
