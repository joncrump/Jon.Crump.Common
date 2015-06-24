namespace TightlyCurly.Com.Common.Data.Repositories.Strategies
{
    public interface IBuilderStrategyFactory
    {
        IBuilderStrategy GetStrategy(BuildMode buildMode);
    }
}
