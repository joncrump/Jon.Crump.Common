namespace TightlyCurly.Com.Common.Data.Repositories
{
    public interface IReadRepository<out TInterface, in TIdType>
    {
        TInterface GetById(TIdType id);
    }
}
