
namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies
{
    public interface IQueryBuilderStrategy
    {
        QueryInfo BuildQuery<TValue>(dynamic parameters = null) 
            where TValue : class;
    }
}
