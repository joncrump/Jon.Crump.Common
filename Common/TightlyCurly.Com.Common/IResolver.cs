using System;

namespace TightlyCurly.Com.Common
{
    public interface IResolver
    {
        TValue Resolve<TValue>();
        object Resolve(Type type);
        TValue Resolve<TValue>(string key);
    }
}
