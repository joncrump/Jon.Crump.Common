namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public interface IAsserterStrategyFactory
    {
        IAsserter Asserter { get; set; }
        IAsserterStrategy GetStrategy<TValue>();
    }
}
