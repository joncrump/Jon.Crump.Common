using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public abstract class WriteDatabaseRepositoryBase<TInterface, TModel> 
        : ReadDatabaseRepositoryBase<TInterface, TModel> 
        where TModel : class, TInterface, new()
    {
        protected WriteDatabaseRepositoryBase(string databaseName, 
            IDatabaseFactory databaseFactory, IMapper mapper, 
            IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) 
            : base(databaseName, databaseFactory, mapper, queryBuilder, 
                builderStrategyFactory)
        {
        }

        public virtual TInterface Save(TInterface model, bool isNew,
            Action<TInterface> insertAction, Action<TInterface> updateAction,
            Expression updateExpression)
        {
            Guard.EnsureIsNotNull("model", model);

            var mappedModel = Mapper.Map<TModel>(model);

            if (isNew)
            {
                if (insertAction.IsNotNull())
                {
                    insertAction(mappedModel);
                }

                ExecuteNonQuery(QueryBuilder.BuildInsertQuery(mappedModel, false));
            }
            else
            {
                if (updateAction.IsNotNull())
                {
                    updateAction(mappedModel);
                }

                if (updateExpression.IsNull())
                {
                    throw new InvalidOperationException("Don't know how to update model.  Update expression is null");
                }

                ExecuteNonQuery(QueryBuilder.BuildUpdateQuery(mappedModel, updateExpression as Expression<Func<TModel, bool>>));
            }

            return mappedModel;
        }

        public virtual void Delete(TInterface model, Expression<Func<TModel, bool>> deleteExpression = null)
        {
            Guard.EnsureIsNotNull("model", model);

            ExecuteNonQuery(QueryBuilder.BuildDeleteQuery(deleteExpression));
        }
    }
}