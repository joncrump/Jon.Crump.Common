using System;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface IEnumParser
    {
        T Parse<T>(string value);
        object Parse(Type type, string value);
    }
}