using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TightlyCurly.Com.Framework.Extensions;

namespace TightlyCurly.Com.Framework.Helpers
{
    public class ReflectionHelper : IReflectionHelper
    {
        public IEnumerable<PropertyInfo> GetPropertiesFromAttribute<TAttribute>(TAttribute attribute, Type type) 
            where TAttribute : Attribute
        {
            Guard.EnsureIsNotNull("attribute", attribute);
            Guard.EnsureIsNotNull("type", type);

            var typeProperties = type.GetProperties();

            return (typeProperties.Select(
                property =>
                new
                {
                    property,
                    temp =
                    property.GetCustomAttributes(typeof(ObsoleteAttribute), false).FirstOrDefault() as
                    ObsoleteAttribute
                }).Where(@t => @t.temp != null).Select(@t => @t.property)).ToSafeList();
        }
    }
}
