using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;

namespace TightlyCurly.Com.Common.Data.QueryBuilders
{
    public interface IQueryBuilderStrategyFactory
    {
        IQueryBuilderStrategy GetBuilderStrategy(QueryKind queryKind);
    }
}
