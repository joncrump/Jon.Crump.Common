using TightlyCurly.Com.Common.Data.Repositories;
using TightlyCurly.Com.Common.Models;
using TightlyCurly.Com.Repositories.Models;

namespace TightlyCurly.Com.Repositories
{
    public interface ICategoryTypeRepository : IWriteRepository<ICategoryType>, 
        IReadRepository<ICategoryType, CategoryTypeDataModel>, ICriteriaRepository<ICategoryType>,
        IPagingRepository
    {
    }
}
