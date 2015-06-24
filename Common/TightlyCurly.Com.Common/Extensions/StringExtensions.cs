using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TightlyCurly.Com.Common.Extensions
{
    public static class StringExtensions
    {
        public static Uri ToUri(string value, UriKind uriKind)
        {
            Uri uri;

            Uri.TryCreate(value, UriKind.RelativeOrAbsolute, out uri);

            return uri;
        }

        public static string ToSafeString(this object value, bool convertNullToEmpty = false)
        {
            if (value.IsNull())
            {
                return !convertNullToEmpty ? null : String.Empty;
            }

            return value.ToString();
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

        public static string SafeTrim(this string value)
        {
            return value.IsNullOrEmpty() ? String.Empty : value.Trim();
        }

        public static string Remove(this string value, string stringToRemove)
        {
            if (!String.IsNullOrEmpty(value) && !(String.IsNullOrEmpty(stringToRemove))
                && value.IndexOf(stringToRemove) >= 0)
            {
                value = value.Replace(stringToRemove, String.Empty);
            }

            return value;
        }

        public static bool IsUppercase(this string str)
        {
            //variable to hold our return value
            bool upper;
            //variable to hold our search pattern
            const string pattern = "[a-z]";

            try
            {
                var allCaps = new Regex(pattern);
                
                if (allCaps.IsMatch(str))
                {
                    upper = false;
                }

                upper = true;
            }
            catch
            {
                upper = false;
            }

            return upper;
        } 
    }
}
