using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common
{
    public class ConstructorTester //: TestBase
    {
        private int _nullIndex = 0;
        private readonly IDataGenerator _dataGenerator;

        public ConstructorTester(IDataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }

        public void TestConstructorsForNullParameters<TItem>() where TItem : class
        {
            var itemType = typeof(TItem);

            var constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Where(c => c.GetParameters().IsNotNullOrEmpty());

            var failedParameters = new List<string>();

            foreach (var constructor in constructors)
            {
                var failedConstructorParameters = TestConstructorForNullParameter(itemType, constructor);

                if (failedConstructorParameters.IsNotNullOrEmpty())
                {
                    failedParameters.AddRange(failedConstructorParameters
                        .Where(p => !failedParameters.Contains(p))
                        .ToList());
                }
            }

            if (failedParameters.IsNotNullOrEmpty())
            {
                throw new AssertException("{0}\r\n{1}".FormatString("The following parameters failed the null check:",
                    failedParameters.ToDelimitedString("\r\n")));
            }
        }

        private IEnumerable<string> TestConstructorForNullParameter(Type itemType, ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters().ToList();
            var failedParameters = new List<string>();

            for (var index = 0; index < parameters.Count; index++)
            {
                var argumentNullExceptionThrown = false;
                _nullIndex = index;
                var parameterInstances = InitializeParameters(parameters);

                try
                {
                    Activator.CreateInstance(itemType, parameterInstances.ToArray());
                }
                catch (Exception exception)
                {
                    var innerException = exception.InnerException as ArgumentNullException;

                    if (innerException.IsNull())
                    {
                        throw;
                    }

                    argumentNullExceptionThrown = true;
                }
                finally
                {
                    if (!argumentNullExceptionThrown)
                    {
                        failedParameters.Add(parameters[index].Name);
                    }
                }
            }

            return failedParameters;
        }

        protected IEnumerable<object> InitializeParameters(IList<ParameterInfo> parameters)
        {
            var instances = new List<object>();

            for (var index = 0; index < parameters.Count; index++)
            {
                if (index == _nullIndex)
                {
                    instances.Add(null);
                }
                else
                {
                    var type = parameters[index].ParameterType;

                    if (type == typeof(string))
                    {
                        instances.Add(_dataGenerator.GenerateString());
                    }
                    else
                    {
                        var mockType = typeof(Mock<>).MakeGenericType(parameters[index].ParameterType);

                        var mock = Convert.ChangeType(Activator.CreateInstance(mockType,
                                                                               null), mockType);

                        instances.Add(GetObjectFromMock(mock, parameters[index].ParameterType));
                    }
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
