using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Data;

namespace TightlyCurly.Com.Providers.Common
{
    public interface IExportableProvider<out TInterface>
    {
        DatatableObjectMapping ToDataTable(Expression filterExpression = null, PagingInfo pagingInfo = null);
        void TransformData(Action<TInterface> transformAction, Expression filterExpression = null,
            PagingInfo pagingInfo = null, bool includeParameters = true);
    }
}
