using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;

namespace TightlyCurly.Com.Common.Data.Tests.DatabaseRepositoryBaseTests
{
    public class TestableDatabaseRepository : DatabaseRepositoryBase
    {
        public TestableDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, 
            IBuilderStrategyFactory builderDelegateStrategyFactory) 
            : base(databaseName, databaseFactory, builderDelegateStrategyFactory)
        {
        }
    }
}
