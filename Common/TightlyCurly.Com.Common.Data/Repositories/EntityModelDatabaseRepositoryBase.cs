using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.QueryBuilders;
using TightlyCurly.Com.Common.Data.Repositories.Strategies;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public abstract class EntityModelDatabaseRepositoryBase<TInterface, TModel> :
            WriteDatabaseRepositoryBase<TInterface, TModel>
        where TModel : class, TInterface, new()
        where TInterface : IModel
    {
        protected EntityModelDatabaseRepositoryBase(
            string databaseName, IDatabaseFactory databaseFactory, IMapper mapper, 
            IQueryBuilder queryBuilder, IBuilderStrategyFactory builderStrategyFactory) 
            : base(databaseName, databaseFactory, mapper, queryBuilder, builderStrategyFactory)
        {
        }

        public virtual TInterface GetById(Guid id)
        {
            Guard.EnsureIsValid("id", i => i != Guid.Empty, id);

            return GetSingle(m => m.Id == id);
        }

        public virtual TInterface GetById(TModel model)
        {
            Guard.EnsureIsNotNull("model", model);

            return GetById(model.Id);
        }

        public override TInterface Save(TInterface model, bool isNew, 
// ReSharper disable OptionalParameterHierarchyMismatch
            Action<TInterface> insertAction = null, 
            Action<TInterface> updateAction = null, 
            Expression updateExpression = null
            // ReSharper restore OptionalParameterHierarchyMismatch
            )
        {
            Guard.EnsureIsNotNull("model", model);

            var mappedModel = Mapper.Map<TModel>(model);

            if (insertAction.IsNotNull() && updateAction.IsNotNull() && updateExpression.IsNotNull())
            {
                return base.Save(mappedModel, isNew, insertAction, updateAction, updateExpression);
            }

            Expression<Func<TModel, bool>> filterExpression = m => m.Id == mappedModel.Id;

            return base.Save(model, model.IsNew(), m =>
            {
                m.Id = Guid.NewGuid();
                m.EnteredDate = DateTime.Now;
                m.UpdatedDate = DateTime.Now;
            }, m =>
            {
                m.UpdatedDate = DateTime.Now;
            }, filterExpression);
        }

        public void Delete(TInterface model)
        {
            Guard.EnsureIsNotNull("model", model);

            Delete(model, m => m.Id == model.Id);
        }
    }
}
