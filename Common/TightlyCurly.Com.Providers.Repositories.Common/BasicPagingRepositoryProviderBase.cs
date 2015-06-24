using System.Linq.Expressions;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Providers.Repositories.Common
{
    public abstract class BasicPagingRepositoryProviderBase<TInterface, TModel, TIdType, TRepository> 
        : BasicCriteriaRepositoryProviderBase<TInterface, TModel, TIdType, TRepository>
        where TRepository : IReadRepository<TInterface, TIdType>, ICriteriaRepository<TInterface>, IPagingRepository
        where TModel : TInterface
    {
        protected BasicPagingRepositoryProviderBase(TRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public virtual PagingModel GetPageByCriteria(Expression filterExpression = null, PagingInfo pagingInfo = null)
        {
            return Repository.GetCriteriaByPage(filterExpression, pagingInfo);
        }
    }
}
