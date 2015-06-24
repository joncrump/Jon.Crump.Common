using System;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class NumberUtilities
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool IsNumeric(string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return (IsInt32(value) || IsInt64(value) || IsDecimal(value) || IsFloat(value)
                        || IsLong(value));
            }

            return false;
        }

        private static bool IsInt32(string value)
        {
            int result;
            return Int32.TryParse(value, out result);
        }

        public static bool IsInt64(string value)
        {
            long result;
            return Int64.TryParse(value, out result);
        }

        public static bool IsDecimal(string value)
        {
            decimal result;
            return Decimal.TryParse(value, out result);
        }

        public static bool IsFloat(string value)
        {
            float result;
            return float.TryParse(value, out result);
        }

        public static bool IsLong(string value)
        {
            long result;
            return long.TryParse(value, out result);
        }

        #endregion
    }
}
