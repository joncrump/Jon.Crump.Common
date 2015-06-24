using System;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Common.Data
{
    public interface IPredicateBuilder
    {
        QueryContainer BuildContainer(Expression expression, Type declaringType, bool addParameters = true, 
            string tableAlias = null, string fieldPrefix = null);
    }
}
