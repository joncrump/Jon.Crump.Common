using System;

namespace TightlyCurly.Com.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EnumAttribute : Attribute
    {
        public EnumAttribute(Type enumType, Type baseType, string fieldMapping)
        {
            EnumType = Guard.EnsureIsNotNull("enumType", enumType);
            BaseType = Guard.EnsureIsNotNull("baseType", baseType);
            FieldMapping = Guard.EnsureIsNotNullOrEmpty("fieldMapping", fieldMapping);
        }

        public Type EnumType { get; private set; }
        public Type BaseType { get; private set; }
        public string FieldMapping { get; private set; }
    }
}
