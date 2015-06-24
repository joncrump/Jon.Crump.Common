using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IIngredient : IModel
    {
        IEnumerable<IIngredientCategory> IngredientCategories { get; set; }
        string Title { get; set; }
        IEnumerable<string> Aliases { get; set; }
        string Description { get; set; }
        IEnumerable<Uri> Links { get; set; }
        IEnumerable<IIngredientRating> IngredientRatings { get; set; }
        IEnumerable<string> Notes { get; set; }
        IEnumerable<IIngredientComment> Comments { get; set; }
        IEnumerable<IContentArticle> ContentArticles { get; set; }
    }
}
