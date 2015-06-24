using System;

namespace TightlyCurly.Com.Providers.Common
{
    public interface IReadProvider<out TInterface, TIdType>
    {
        TInterface GetById(TIdType id, Func<TIdType, bool> parameterValidationFunction = null);
    }
}
