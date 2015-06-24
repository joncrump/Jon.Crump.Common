using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TightlyCurly.Com.Framework.Attributes.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class MaximumDateAttribute : Attribute
    {
        #region Fields

        #endregion

        #region Constructors

        public MaximumDateAttribute(DateTime value)
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
