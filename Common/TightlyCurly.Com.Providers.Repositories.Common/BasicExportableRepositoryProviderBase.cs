using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;
using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Providers.Repositories.Common
{
    public abstract class BasicExportableRepositoryProviderBase<TInterface, TModel, TIdType, TRepository> 
        : BasicPagingRepositoryProviderBase<TInterface, TModel, TIdType, TRepository>
        where TRepository : IReadRepository<TInterface, TIdType>, ICriteriaRepository<TInterface>, IPagingRepository, IExportableRepository<TInterface>
        where TModel : TInterface
    {
        protected BasicExportableRepositoryProviderBase(TRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public virtual DatatableObjectMapping ToDataTable(Expression filterExpression = null, PagingInfo pagingInfo = null)
        {
            return Repository.ToDataTable(filterExpression, pagingInfo);
        }

        public virtual void TransformData(Action<TInterface> transformAction, Expression filterExpression = null,
            PagingInfo pagingInfo = null, bool includeParameters = true)
        {
            Repository.TransformData(transformAction, filterExpression, pagingInfo, includeParameters);
        }
    }
}
