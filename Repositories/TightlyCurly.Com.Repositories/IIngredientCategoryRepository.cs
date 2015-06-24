using System.Collections.Generic;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories
{
    public interface IIngredientCategoryRepository
    {
        IEnumerable<IIngredientCategory> GetAllIngredientCategories();
        IEnumerable<IIngredientCategory> GetIngredientCategoriesByIngredient(IIngredient ingredient);
        IEnumerable<IIngredientCategory> GetIngredientCategoriesByCategoryType(ICategoryType categoryType);
        IIngredientCategory GetIngredientCategoryById(int id, ILocale locale);
        IIngredientCategory SaveIngredientCategory(IIngredientCategory ingredientCategory);
        void DeleteIngredientCategory(IIngredientCategory ingredientCategory);
    }
}
