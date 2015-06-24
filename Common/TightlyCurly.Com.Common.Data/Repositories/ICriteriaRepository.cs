using System.Collections.Generic;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public interface ICriteriaRepository<out TInterface>
    {
        IEnumerable<TInterface> GetByCriteria(Expression filterExpression = null);
    }
}
