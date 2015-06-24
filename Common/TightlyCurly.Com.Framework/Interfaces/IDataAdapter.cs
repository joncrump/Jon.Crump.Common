using System.Data.Common;

namespace TightlyCurly.Com.Framework.Data.Interfaces
{
    public interface IDataAdapter<T>
    {
        T Load(T dataObject, string columnPrefix, DbDataReader reader);
        T LoadFromId(int id);
        T Save(T dataObject);
        void Delete(T dataObject);
    }
}
