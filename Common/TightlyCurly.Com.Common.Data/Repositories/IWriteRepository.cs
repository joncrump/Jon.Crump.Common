using System;
using System.Linq.Expressions;

namespace TightlyCurly.Com.Common.Data.Repositories
{
    public interface IWriteRepository<TInterface>
    {
        TInterface Save(TInterface model, bool isNew,
            Action<TInterface> insertAction = null,
            Action<TInterface> updateAction = null,
            Expression updateExpression = null);
        void Delete(TInterface model);
    }
}
