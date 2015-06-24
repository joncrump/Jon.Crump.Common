namespace TightlyCurly.Com.Common.Data.QueryBuilders.Strategies
{
    public abstract class QueryBuilderStrategyBase
    {
        protected readonly IFieldHelper FieldHelper;

        protected QueryBuilderStrategyBase(IFieldHelper fieldHelper)
        {
            FieldHelper = Guard.EnsureIsNotNull("fieldHelper", fieldHelper);
        }
    }
}
