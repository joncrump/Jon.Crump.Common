using System.Linq.Expressions;
using TightlyCurly.Com.Common;

namespace TightlyCurly.Com.Providers.Common
{
    public interface IPagingProvider
    {
        PagingModel GetPageByCriteria(Expression filterExpression = null, PagingInfo pagingInfo = null);
    }
}
