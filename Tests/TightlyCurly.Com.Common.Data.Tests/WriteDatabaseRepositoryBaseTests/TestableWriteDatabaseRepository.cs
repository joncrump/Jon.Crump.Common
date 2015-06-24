using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Tests.WriteDatabaseRepositoryBaseTests
{
    public class TestableWriteDatabaseRepository : WriteDatabaseRepositoryBase<ITestModel, TestModel>
    {
        public TestableWriteDatabaseRepository(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }
    }
}
