using System;
using System.Collections.Generic;
using System.Reflection;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface IReflectionHelper
    {
        IEnumerable<PropertyInfo> GetPropertiesFromAttribute<TAttribute>(TAttribute attribute, Type type)
            where TAttribute : Attribute;

    }
}
