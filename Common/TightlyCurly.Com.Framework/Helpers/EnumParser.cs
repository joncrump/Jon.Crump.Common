using System;

namespace TightlyCurly.Com.Framework.Helpers
{
    public class EnumParser : IEnumParser
    {
        public T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public object Parse(Type type, string value)
        {
            return Enum.Parse(type, value);
        }
    }
}
