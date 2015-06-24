using System.Collections.Generic;
using System.Data;

namespace TightlyCurly.Com.Common.Data.Repositories.Strategies
{
    public interface IBuilderStrategy
    {
        IEnumerable<TValue> BuildItems<TValue>(dynamic parameters, IDataReader dataSource)
             where TValue : class, new(); 
    }
}
