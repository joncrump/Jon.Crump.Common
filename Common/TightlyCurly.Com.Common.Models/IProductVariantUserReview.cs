using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IProductVariantUserReview : IModel
    {
        int? Order { get; set; }
        IUser ReviewedBy { get; set; }
        string Review { get; set; }
        decimal? Rating { get; set; }
        IEnumerable<IAside> Asides { get; set; }
        IEnumerable<IImageUri> ImageUris { get; set; }
        DateTime ReviewDate { get; set; }
        bool? IsActive { get; set; }
    }
}
