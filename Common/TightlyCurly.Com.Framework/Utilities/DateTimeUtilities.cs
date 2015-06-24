using System;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class DateTimeUtilities
    {
        public static object ReturnNullIfEmpty(DateTime value)
        {
            if (value.Equals(DateTime.MinValue))
                return null;

            return value;
        }

        public static DateTime ReturnDateTimeFromNull(object value)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }

            return (DateTime)value;
        }
    }
}
