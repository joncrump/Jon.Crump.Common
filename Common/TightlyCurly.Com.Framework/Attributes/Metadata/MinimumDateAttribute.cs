using System;
using TightlyCurly.Com.Framework.Common;

namespace TightlyCurly.Com.Framework.Attributes.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MinimumDateAttribute : Attribute
    {
        #region Fields

        #endregion

        #region Constructors

        public MinimumDateAttribute()
        {
            Value = Constants.MinAcceptableDate;
        }

        public MinimumDateAttribute(DateTime value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        public DateTime Value { get; private set; }

        #endregion

        #region Methods

        #endregion
    }
}
