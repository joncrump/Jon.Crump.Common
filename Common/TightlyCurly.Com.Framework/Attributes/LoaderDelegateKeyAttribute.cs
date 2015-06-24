using System;

namespace TightlyCurly.Com.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LoaderDelegateKeyAttribute : Attribute
    {
        public LoaderDelegateKeyAttribute(string key)
        {
            Key = Guard.EnsureIsNotNullOrEmpty("key", key);
        }

        public string Key { get; private set; }
    }
}
