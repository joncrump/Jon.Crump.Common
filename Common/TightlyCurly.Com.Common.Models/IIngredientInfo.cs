namespace TightlyCurly.Com.Common.Models
{
    public interface IIngredientInfo : IModel
    {
        IIngredient Ingredient { get; set; }
        int? Order { get; set; }
        bool? IsActive { get; set; }
    }
}
