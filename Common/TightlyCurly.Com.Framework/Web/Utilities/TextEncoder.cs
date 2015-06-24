using System;
using System.Text.RegularExpressions;
using System.Web;
//using Microsoft.Security.Application;
using TightlyCurly.Com.Framework.Utilities;

namespace TightlyCurly.Com.Framework.Web.Utilities
{
    public static class TextEncoder
    {
        #region Fields

        private const string ScriptObjectPattern = "<script.*?>.*?</script>";

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static string SafeEncode(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                value = Regex.Replace(value, ScriptObjectPattern, String.Empty, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                value = HttpContext.Current.Server.HtmlEncode(value);
            }

            return value;
        }

        #endregion
    }
}
