using System;
using System.Collections.Generic;

namespace TightlyCurly.Com.Framework.Extensions
{
    public static class StringExtensions
    {
        public static Uri ToUri(string value, UriKind uriKind)
        {
            Uri uri;

            Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out uri);

            return uri;
        }

        public static Uri ToUri(this string value)
        {
            return ToUri(value, UriKind.RelativeOrAbsolute);
        }

        public static string ToDelimitedString(this IEnumerable<string> value, string delimiter)
        {
            return value.IsNullOrEmpty() 
                ? String.Empty 
                : String.Join(delimiter, value);
        }

        public static string FormatString(this string value, params object[] parameters)
        {
            if (value.IsNullOrEmpty())
            {
                return String.Empty;
            }

            return String.Format(value, parameters);
        }
    }
}
