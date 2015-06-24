using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Models
{
    public interface IIngredientCategory : IModel
    {
        string Name { get; set; }
        IEnumerable<ICategoryType> CategoryTypes { get; set; }
    }
}
