using System;
using System.Text.RegularExpressions;
using TightlyCurly.Com.Framework.Common;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class StringUtility
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool IsEmailAddress(this string value)
        {
            return (!String.IsNullOrEmpty(value) && Regex.IsMatch(value, Constants.EmailAddressPattern, RegexOptions.Compiled));
        }

        public static string Remove(string value, string stringToRemove)
        {
            if (!String.IsNullOrEmpty(value) && !(String.IsNullOrEmpty(stringToRemove))
                && value.IndexOf(stringToRemove) >= 0)
            {
                value = value.Replace(stringToRemove, String.Empty);
            }

            return value;
        }

        //public static string ToProperCase(this string value)
        //{
        //    return ToProperCase(value, CaseOptions.None);
        //}

        //public static string ToProperCase(this string value, CaseOptions options)
        //{
        //    if (!String.IsNullOrEmpty(value))
        //    {
        //        string inputString = value;
        //        string tmpStr;
        //        string tmpChar;
        //        string preString;
        //        string postString;
        //        int stringLength;

        //        tmpStr = (options == CaseOptions.IgnoreUpperCase ? inputString : inputString.ToLower());
        //        stringLength = tmpStr.Length;

        //        if (stringLength > 0)
        //        {
        //            for (int i = 0; i < stringLength; i++)
        //            {
        //                if (i == 0)
        //                {
        //                    tmpChar = tmpStr.Substring(0, 1).ToUpper();
        //                    postString = tmpStr.Substring(1, stringLength - 1);//stringLength);
        //                    tmpStr = tmpChar + postString;
        //                }
        //                else
        //                {
        //                    tmpChar = tmpStr.Substring(i, i + 1);

        //                    if (tmpChar == " " && i < (stringLength - 1))
        //                    {
        //                        tmpChar = tmpStr.Substring(i + 1, i + 2).ToUpper();
        //                        preString = tmpStr.Substring(0, i + 1);
        //                        postString = tmpStr.Substring(i + 2, stringLength);
        //                        tmpStr = preString + tmpChar + postString;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return value;
        //}

        public static bool IsUppercase(string str)
        {
            //variable to hold our return value
            bool upper;
            //variable to hold our search pattern
            string pattern = "[a-z]";
            try
            {
                Regex AllCaps = new Regex(pattern);
                if (AllCaps.IsMatch(str))
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


        #endregion
    }
}
