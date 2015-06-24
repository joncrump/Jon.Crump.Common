using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public interface IBulkRepository<in TInterface>
    {
        void InsertBulk(IEnumerable<TInterface> models);
    }
}
