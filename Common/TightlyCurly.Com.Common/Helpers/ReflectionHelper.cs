using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Helpers
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

        public bool IsTypeGenericEnumerable(Type candidateType, Type genericType)
        {
            throw new NotImplementedException();
            //if (typeInterface.IsGenericType
            //        && typeInterface.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            //{
            //    var implementType = typeInterface.GetGenericArguments().FirstOrDefault();

            //    if (implementType.IsInterface &&
            //        childType.GetInterfaces().SafeWhere(i => i == implementType).IsNullOrEmpty()
            //        || (implementType == childType))
            //    {
            //        return true;
            //    }
            //}

            //return false;
        }
    }
}
