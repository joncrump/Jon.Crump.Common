using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies;

namespace TightlyCurly.Com.Common.Data.QueryBuilders
{
    public class QueryBuilderStrategyFactory : IQueryBuilderStrategyFactory
    {
        private readonly IResolver _resolver;

        public QueryBuilderStrategyFactory(IResolver resolver)
        {
            _resolver = Guard.EnsureIsNotNull("resolver", resolver);
        }

        public IQueryBuilderStrategy GetBuilderStrategy(QueryKind queryKind)
        {
            return _resolver.Resolve<IQueryBuilderStrategy>(queryKind.ToString());
        }
    }
}
