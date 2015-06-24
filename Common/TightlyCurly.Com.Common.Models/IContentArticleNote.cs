namespace TightlyCurly.Com.Common.Models
{
    public interface IContentArticleNote : IModel
    {
        IUser EnteredBy { get; set; }
        string Note { get; set; }
    }
}
