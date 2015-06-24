namespace TightlyCurly.Com.Common.Models
{
    public interface IMessageTemplateToken : IModel
    {
        string Key { get; set; }
        string Value { get; set; }
    }
}
