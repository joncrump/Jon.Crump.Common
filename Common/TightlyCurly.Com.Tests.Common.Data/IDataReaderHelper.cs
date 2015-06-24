using System.Collections.Generic;

namespace TightlyCurly.Com.Tests.Common.Data
{
    public interface IDataReaderHelper
    {
        MockDataReader BuildMockDataReader<TModel>(IEnumerable<TModel> instances, 
            string prefix = null) where TModel : class, new();
    }
}
