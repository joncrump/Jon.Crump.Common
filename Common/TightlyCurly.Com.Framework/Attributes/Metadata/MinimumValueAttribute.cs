using System;

namespace TightlyCurly.Com.Framework.Attributes.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MinimumValueAttribute : Attribute
    {
        #region Fields

        #endregion

        #region Constructors

        public MinimumValueAttribute(int minimumValue)
        {
            MinimumValue = minimumValue;
        }

        #endregion

        #region Properties

        public int MinimumValue { get; private set; }

        #endregion

        #region Methods

        #endregion
    }
}
