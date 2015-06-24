using System.Linq.Expressions;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public interface IPagingRepository
    {
        PagingModel GetCriteriaByPage(Expression filterExpression = null, PagingInfo pagingInfo = null,
            bool includeParameters = true);
    }
}
