using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Providers.Repositories.Common
{
    public abstract class BasicWriteRepositoryProviderBase<TInterface, TModel, TIdType, TRepository> 
        : BasicExportableRepositoryProviderBase<TInterface, TModel, TIdType, TRepository> 
        where TRepository : IReadRepository<TInterface, TIdType>, IWriteRepository<TInterface>, ICriteriaRepository<TInterface>, IPagingRepository, IExportableRepository<TInterface>
        where TModel : TInterface
    {
        protected BasicWriteRepositoryProviderBase(TRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public virtual TInterface Save(TInterface model, bool isNew, 
            Action<TInterface> insertAction = null,
            Action<TInterface> updateAction = null,
            Expression updateExpression = null)
        {
            Guard.EnsureIsNotNull("model", model);

            return Mapper.Map<TModel>(Repository.Save(model, isNew, insertAction, updateAction, updateExpression));
        }

        public virtual void Delete(TInterface model)
        {
            Guard.EnsureIsNotNull("model", model);

            Repository.Delete(model);
        }
    }
}
