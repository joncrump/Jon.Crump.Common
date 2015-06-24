using System.Collections.ObjectModel;

namespace TightlyCurly.Com.Framework.Utilities
{
    public static class CollectionUtilites<CollectionType, CollectionMemberType> where CollectionType : Collection<CollectionMemberType>
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static bool IsNullOrEmpty(CollectionType value)
        {
            return (value == null || value.Count > 0);
        }

        #endregion

    }
}
