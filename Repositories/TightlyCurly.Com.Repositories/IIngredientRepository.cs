using System.Collections.Generic;
using TightlyCurly.Com.Common.Models;

namespace TightlyCurly.Com.Repositories
{
    public interface IIngredientRepository
    {
        IEnumerable<IIngredient> GetAllIngredients();
        IEnumerable<IIngredient> GetIngredientsByRating(IIngredientRating rating, ILocale locale);
        IIngredient GetIngredientById(int id, ILocale locale);
        IIngredient GetIngredientByTitle(string title, ILocale locale);
        IIngredient SaveIngredient(IIngredient ingredient);
        void DeleteIngredient(IIngredient ingredient);
    }
}
