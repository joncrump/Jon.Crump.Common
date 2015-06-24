using System.Collections.Generic;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Base;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public class ReflectionBasedObjectCreator : UtilityBase, IObjectCreator
    {
        public ReflectionBasedObjectCreator() : this(new RandomDataGenerator())
        {
        }

        public ReflectionBasedObjectCreator(IDataGenerator dataGenerator) : base(dataGenerator)
        {
        }

        public T CreateNew<T>(IDictionary<string, object> parameters) where T : class, new()
        {
            var value = new T();

            var properties = value.GetType().GetProperties();

            foreach (var property in properties)
            {
                object propertyValue;

                if (!parameters.IsNullOrEmpty() && parameters.SafeGet(property.Name).IsNotNull())
                {
// ReSharper disable PossibleNullReferenceException
                    propertyValue = parameters[property.Name];
// ReSharper restore PossibleNullReferenceException
                }
                else
                {
                    var key = property.PropertyType;

                    propertyValue = Mappings.ContainsKey(key) 
                        ? Mappings[key]()
                        : null;
                }
                
                property.SetValue(value, propertyValue, null);
            }
            
            return value;
        }
    }
}
