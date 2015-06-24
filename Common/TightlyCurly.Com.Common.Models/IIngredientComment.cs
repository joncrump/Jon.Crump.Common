using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IIngredientComment : IModel
    {
        string Comment { get; set; }
        ILocale Locale { get; set; }
        Guid? ParentId { get; set; }
        IUser User { get; set; }
        IEnumerable<IIngredientComment> Children { get; set; }
        Guid IngredientId { get; set; }
    }
}