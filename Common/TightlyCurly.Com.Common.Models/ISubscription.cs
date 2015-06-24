namespace TightlyCurly.Com.Common.Models
{
    public interface ISubscription : IModel
    {
        string Name { get; set; }
        string Description { get; set; }
        bool IsPrivate { get; set; }
    }
}
