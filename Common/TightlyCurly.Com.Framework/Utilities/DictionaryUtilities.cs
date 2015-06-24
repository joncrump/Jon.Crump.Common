using System.Collections.Generic;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class DictionaryUtilities<DictionaryType, DictionaryKeyType, DictionaryValueType> 
        where DictionaryType : Dictionary<DictionaryKeyType, DictionaryValueType> 
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool IsNullOrEmpty(DictionaryType value)
        {
            return (value == null || value.Keys.Count == 0);
        }

        #endregion
    }
}
