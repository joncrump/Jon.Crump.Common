using System;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class GuidUtilities
    {
        public static object ReturnNullIfEmpty(Guid value)
        {
            if (value.Equals(Guid.Empty))
                return null;

            return value;
        }

        public static Guid? ReturnGuidFromNull(object value)
        {
            if (value == null)
            {
                return Guid.Empty;
            }

            return (Guid)value;
        }

        public static bool TryParse(string value, out Guid guidValue)
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
