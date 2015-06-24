using System.Collections.Generic;

namespace TightlyCurly.Com.Common.Data
{
    public interface IModelDataConverter
    {
        IDatatableObjectMapping ConvertToDataTable<TModel>(IEnumerable<TModel> models);
    }
}
