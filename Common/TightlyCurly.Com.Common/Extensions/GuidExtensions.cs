using System;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class GuidUtilities
    {
        public static object ReturnNullIfEmpty(this Guid value)
        {
            if (value.Equals(Guid.Empty))
            {
                return null;
            }

            return value;
        }

        public static Guid? ReturnGuidFromNull(this object value)
        {
            if (value == null)
            {
                return Guid.Empty;
            }

            return (Guid)value;
        }

        public static bool TryParse(this string value, out Guid guidValue)
        {
            try
            {
                guidValue = new Guid(value);
                return true;
            }
            catch
            {
                guidValue = Guid.Empty;
                return false;
            }
        }
    }
}
