using System;

namespace TightlyCurly.Com.Framework.Attributes.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MaximumValueAttribute : Attribute
    {
        #region Fields

        #endregion

        #region Constructors

        public MaximumValueAttribute(int maximumValue)
        {
            MaximumValue = maximumValue;
        }

        #endregion

        #region Properties

        public int MaximumValue { get; private set; }

        #endregion

        #region Methods

        #endregion
    }
}
