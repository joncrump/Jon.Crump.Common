using System;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Providers.Common;

namespace TightlyCurly.Com.Providers.Repositories.Common
{
    public abstract class BasicReadRepositoryProviderBase<TInterface, TModel, TIdType, TRepository> : BasicProviderBase
        where TRepository : IReadRepository<TInterface, TIdType>
        where TModel : TInterface
    {
        protected readonly TRepository Repository;

        protected BasicReadRepositoryProviderBase(TRepository repository, IMapper mapper)
            : base(mapper)
        {
            Repository = Guard.EnsureIsNotNull("repository", repository);
        }

        public virtual TInterface GetById(TIdType id, Func<TIdType, bool> parameterValidationFunction = null)
        {
            if (parameterValidationFunction.IsNotNull())
            {
                Guard.EnsureIsValid("id", parameterValidationFunction, id);
            }

            return Mapper.Map<TModel>(Repository.GetById(id));
        }
    }
}
