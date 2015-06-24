using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;

namespace TightlyCurly.Com.Framework.Web.Caching
{
    public static class CacheHelper
    {
        #region Fields

        private const double DefaultExpiration = 20;

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static object RetrieveFromCache(string objectName)
        {
            return HttpRuntime.Cache[objectName];
        }

        public static void InsertCache(string key, object value)
        {
            double minutes;

            if (!Double.TryParse(ConfigurationManager.AppSettings["DefaultCacheExpiration"], out minutes))
            {
                minutes = DefaultExpiration;
            }

            InsertCache(key, value, null, minutes, Cache.NoSlidingExpiration);
        }

        public static void InsertCache(string key, object value, CacheDependency cacheDependency, double absoluteExpiration, 
            TimeSpan slidingExpiration)
        {
            HttpRuntime.Cache.Insert(key, value, cacheDependency, DateTime.Now.AddMinutes(absoluteExpiration),
                                   slidingExpiration);
        }

        public static void RemoveFromCache(string key)
        {
            if (!String.IsNullOrEmpty(key))
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        public static bool IsInCache(string key)
        {
            object value = HttpRuntime.Cache[key];

            return value != null;
        }

        #endregion
    }
}
