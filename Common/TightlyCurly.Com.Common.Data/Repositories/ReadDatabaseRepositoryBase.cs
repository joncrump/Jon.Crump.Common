using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public abstract class ReadDatabaseRepositoryBase<TInterface, TModel> : DatabaseRepositoryBase
        where TModel : class, TInterface, new()
    {
        protected readonly IMapper Mapper;
        protected readonly IQueryBuilder QueryBuilder;

         protected ReadDatabaseRepositoryBase(string databaseName, IDatabaseFactory databaseFactory, IMapper mapper,
            IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory)
            : base(databaseName, databaseFactory, builderStrategyFactory)
        {
            Mapper = Guard.EnsureIsNotNull("mapper", mapper);
            QueryBuilder = Guard.EnsureIsNotNull("queryBuilder", queryBuilder);
        }

        public virtual IEnumerable<TInterface> Get(Expression<Func<TModel, bool>> filterExpression = null, bool includeParameters = true, 
            BuildMode buildMode = BuildMode.Single)
        {
            var values =
                ExecuteMultiple<TModel>(QueryBuilder.BuildSelectQuery(filterExpression, true, includeParameters),
                    buildMode);

            return values;
        }

        public virtual TInterface GetSingle(Expression<Func<TModel, bool>> expression, BuildMode buildMode = BuildMode.Single)
        {
            var values = Get(expression, buildMode:buildMode);

            return values.Do(() => values.FirstOrDefault());
        }

        public virtual IEnumerable<TInterface> GetByCriteria(Expression filterExpression = null)
        {
            if (filterExpression.IsNull())
            {
                return Get();
            }

            var castExpression = filterExpression as Expression<Func<TModel, bool>>;

            if (castExpression.IsNull())
            {
                throw new InvalidOperationException("Type of predicate parameter must be type of {0}"
                    .FormatString(typeof(TModel).ToString()));
            }

            return Get(castExpression);
        }
    }
}
