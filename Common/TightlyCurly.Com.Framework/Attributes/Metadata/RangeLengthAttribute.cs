using System;

namespace TightlyCurly.Com.Framework.Attributes.Metadata
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class RangeLengthAttribute : Attribute
    {
        public RangeLengthAttribute(int minimum, int maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public int Minimum { get; private set; }
        public int Maximum { get; private set; }
    }
}
