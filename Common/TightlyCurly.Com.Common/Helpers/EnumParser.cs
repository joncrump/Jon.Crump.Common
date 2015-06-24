using System;

namespace TightlyCurly.Com.Common.Helpers
{
    public class EnumParser : IEnumParser
    {
        public T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public object Parse(Type type, string value)
        {
            return Enum.Parse(type, value, true);
        }
    }
}
