using System;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class DateTimeUtilities
    {
        public static object ReturnNullIfEmpty(this DateTime value)
        {
            if (value.Equals(DateTime.MinValue))
                return null;

            return value;
        }

        public static DateTime ReturnDateTimeFromNull(this object value)
        {
            if (value == null)
            {
                return DateTime.MinValue;
            }

            return (DateTime)value;
        }
    }
}
