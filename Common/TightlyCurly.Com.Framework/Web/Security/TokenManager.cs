using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TightlyCurly.Com.Framework.Web.Caching;

namespace TightlyCurly.Com.Framework.Web.Security
{
    public static class TokenManager
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static void InsertToken(string key, string token)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException("token");
            }

            CacheHelper.InsertCache(key, token); 
        }

        public static string GetToken(string key)
        {
            return CacheHelper.RetrieveFromCache(key) as string;
        }

        public static void RemoveToken(string key)
        {
            CacheHelper.RemoveFromCache(key);
        }

        #endregion
    }
}
