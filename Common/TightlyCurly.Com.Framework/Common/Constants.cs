using System;

namespace TightlyCurly.Com.Framework.Common
{
    public static class Constants
    {
        #region Fields

        public static string EmailAddressPattern = 
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public static readonly DateTime MinAcceptableDate = new DateTime(1900, 1, 1);

        #endregion
    }
}
