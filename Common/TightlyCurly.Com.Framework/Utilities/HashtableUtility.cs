using System.Collections;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class HashtableUtility
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool IsNullOrEmpty(Hashtable hashTable)
        {
            return (hashTable == null || hashTable.Keys.Count == 0);
        }

        #endregion
    }
}
