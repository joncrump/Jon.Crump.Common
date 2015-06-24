using System.Collections.Generic;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Providers.Common
{
    public interface ICriteriaProvider<out TInterface>
    {
        IEnumerable<TInterface> GetByCriteria(Expression filterExpression = null);
    }
}
