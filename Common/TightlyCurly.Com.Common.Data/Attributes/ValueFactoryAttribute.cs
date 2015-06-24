using System;

namespace TightlyCurly.Com.Common.Data.Attributes
{
    public class ValueFactoryAttribute : Attribute
    {
        public ValueFactoryAttribute(string valueFactoryName)
        {
            ValueFactoryName = Guard.EnsureIsNotNullOrEmpty("valueFactoryName", valueFactoryName);
        }

        public string ValueFactoryName { get; private set; }
    }
}
