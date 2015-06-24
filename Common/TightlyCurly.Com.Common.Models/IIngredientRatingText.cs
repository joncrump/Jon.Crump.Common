using System;

namespace TightlyCurly.Com.Common.Models
{
    public interface IIngredientRatingText
    {
        Guid Id { get; set; }
        string Text { get; set; }
        Guid LocaleId { get; set; }
        ILocale Locale { get; set; }
        DateTime EnteredDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}
