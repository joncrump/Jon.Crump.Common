using System;
using System.Collections.Generic;
using System.Reflection;
using Moq;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common.Base
{
    public abstract class UtilityBase
    {
        protected IDataGenerator DataGenerator;
        protected IDictionary<Type, Func<object>> Mappings; 

        protected UtilityBase(IDataGenerator dataGenerator)
        {
            DataGenerator = dataGenerator;
            Mappings = InitializeMappings();
        }
        
        protected IEnumerable<T> CreateEnumerableOfItems<T>(Func<T> itemCreationDelegate, int numberOfItems = 5)
        {
            Guard.EnsureIsValid("numberOfItems", n => n >= 1, numberOfItems, "numberOfItems must be greater than zero.");

            IList<T> values = new List<T>();

            for (var index = 0; index < numberOfItems; index++)
            {
                values.Add(itemCreationDelegate());
            }

            return values;
        }

        private IDictionary<Type, Func<object>> InitializeMappings()
        {
            var mappings = new Dictionary<Type, Func<object>>
                {
                    {typeof (string), () => DataGenerator.GenerateString()},
                    {typeof (Uri), () => DataGenerator.GenerateUri()},
                    {typeof(Int32), () => DataGenerator.GenerateInteger()},
                    {typeof(int?), () => DataGenerator.GenerateBoolean()
                                             ? DataGenerator.GenerateInteger()
                                             : (object) null
                    },
                    {typeof(long), () => DataGenerator.GenerateLong()},
                    {typeof(long?), () => DataGenerator.GenerateBoolean()
                        ? DataGenerator.GenerateLong()
                        : (object) null},
                    {typeof(double), () => DataGenerator.GenerateDouble()},
                    {typeof(double?), () => DataGenerator.GenerateBoolean()
                        ? DataGenerator.GenerateDouble()
                        : (object) null},
                    {typeof(float), () => DataGenerator.GenerateFloat()},
                    {typeof(float?), () => DataGenerator.GenerateBoolean()
                        ? DataGenerator.GenerateFloat()
                        : (object) null},
                    {typeof(DateTime), () => DataGenerator.GenerateDateTime()},
                    {typeof(DateTime?), () => DataGenerator.GenerateBoolean()
                        ? DataGenerator.GenerateDateTime()
                        : (object)null},
                    {typeof(Guid), () => Guid.NewGuid() }
                };

            return mappings;
        }

        protected virtual IEnumerable<object> InitializeParameters(IList<ParameterInfo> parameters)
        {
            var instances = new List<object>();

            for (var index = 0; index < parameters.Count; index++)
            {
                var type = parameters[index].ParameterType;

                if (type == typeof(string))
                {
                    instances.Add(DataGenerator.GenerateString());
                }
                else
                {
                    var mockType = typeof(Mock<>).MakeGenericType(parameters[index].ParameterType);

                    var mock = Convert.ChangeType(Activator.CreateInstance(mockType,
                                                                            null), mockType);

                    instances.Add(GetObjectFromMock(mock, parameters[index].ParameterType));
                }
            }

            return instances;
        }

        protected object GetObjectFromMock(object mock, Type propertyType)
        {
            var type = mock.GetType();

            var property = type.GetProperty("Object", propertyType);

            return property.GetValue(mock);
        }

    }
}
