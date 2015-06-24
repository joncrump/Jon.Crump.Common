using System;

namespace TightlyCurly.Com.Framework.Attributes.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MaxLengthAttribute : Attribute
    {
        #region Fields

        #endregion

        #region Constructors

        public MaxLengthAttribute(int maxLength)
        {
            MaxLength = maxLength;
        }

        #endregion

        #region Properties

        public int MaxLength { get; private set; }

        #endregion

        #region Methods

        #endregion
    }
}
