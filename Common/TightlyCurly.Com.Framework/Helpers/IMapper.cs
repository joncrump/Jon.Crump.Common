namespace TightlyCurly.Com.Framework.Helpers
{
    public interface IMapper
    {
        TReturn Map<TReturn>(object value);
    }
}
