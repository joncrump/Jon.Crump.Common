namespace TightlyCurly.Com.Common.Models
{
    public interface IContentArticleRevision : IModel
    {
        string VersionNumber { get; set; }
        IContentArticle Article { get; set; }
    }
}
