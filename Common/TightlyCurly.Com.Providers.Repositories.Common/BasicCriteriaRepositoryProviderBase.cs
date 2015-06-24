using System.Collections.Generic;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;

namespace TightlyCurly.Com.Providers.Repositories.Common
{
    public abstract class BasicCriteriaRepositoryProviderBase<TInterface, TModel, TIdType, TRepository> 
        : BasicReadRepositoryProviderBase<TInterface, TModel, TIdType, TRepository>
        where TRepository : IReadRepository<TInterface, TIdType>, ICriteriaRepository<TInterface>
        where TModel : TInterface
    {
        protected BasicCriteriaRepositoryProviderBase(TRepository repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public virtual IEnumerable<TInterface> GetByCriteria(Expression filterExpression = null)
        {
            return Repository.GetByCriteria(filterExpression)
                .SafeSelect(o => (TInterface)Mapper.Map<TModel>(o));
        }
    }
}
