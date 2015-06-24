using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class ObjectAsserterStrategy<TValue> : AsserterStrategyBase<TValue> where TValue : class
    {
        private readonly IAsserter _asserter;

        public override string Name
        {
            get { return Constants.StrategyNames.ObjectAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(object); }
        }

        public ObjectAsserterStrategy(IAssertAdapter assertAdapter, IAsserter asserter)
            : base(assertAdapter)
        {
            _asserter = asserter;
        }

        public override void AssertEquality(TValue expected, TValue actual, IEnumerable<string> propertiesToIgnore = null, 
            IDictionary<string, object> additionalParameters = null, bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);

            if (checkForDefault)
            {
                Assert.IsNotNull(actual);
            }

            if (expected == null || actual == null)
            {
                return;
            }

            var failedProperties = AssertObjectInternal(expected, actual, propertiesToIgnore, additionalParameters, recurseProperties);

            if (!failedProperties.IsNullOrEmpty())
            {
                throw new AssertException("Objects {0} and {1} are not equal.  Unequal properties:\r\n{2}"
                    .FormatString(expected, actual, failedProperties.ToDelimitedString("\r\n")));
            }
        }

        private List<string> AssertObjectInternal<T>(T expected, T actual, IEnumerable<string> propertiesToIgnore, 
            IDictionary<string, object> additionalParameters, bool recurseProperties)
        {
            if (propertiesToIgnore.IsNull())
            {
                propertiesToIgnore = new List<string>();
            }

            var properties = expected
                .GetType()
                .GetProperties()
                .ToList();

            var failedProperties = new List<string>();

            foreach (var property in properties)
            {
                var property1 = property;

                if (propertiesToIgnore.Any(p => p.ToLower() == property1.Name.ToLower()))
                {
                    continue;
                }

                object expectedValue = null;
                object actualValue = null;

                try
                {
                    expectedValue = property.GetValue(expected, null);
                    actualValue = property.GetValue(actual, null);

                    if (expectedValue.IsNull() && !actualValue.IsNull())
                    {
                        failedProperties.Add("Property: {0}. Expected: <NULL>, Actual: {1}"
                            .FormatString(property.Name, actualValue));
                    }
                    else if (!expectedValue.IsNull() && actualValue.IsNull())
                    {
                        failedProperties.Add("Property: {0}. Expected: {1}, Actual: <NULL>"
                            .FormatString(property.Name, expectedValue));
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        _asserter.AssertEquality((string)expectedValue, (string)actualValue, 
                            additionalParameters:new Dictionary<string, object>
                        {
                            {Constants.ParameterNames.CheckForNullOrEmpty, false}
                        });
                    }
                    else if (typeof(IEnumerable<T>).IsAssignableFrom(property.PropertyType))
                    {
                        _asserter.AssertEquality((IEnumerable<T>)expectedValue, (IEnumerable<T>)actualValue);
                    }
                    else if (property.PropertyType == typeof(Int32))
                    {
                        _asserter.AssertEquality(Convert.ToInt32(expectedValue), Convert.ToInt32(actualValue), 
                            additionalParameters:new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForNonZero, false},
                                {Constants.ParameterNames.CheckForPositive, false}
                            });
                    }
                    else if (property.PropertyType == typeof(Int32?))
                    {
                        Expression<Action<int, int>> expression = (e, a) =>
                                _asserter.AssertEquality(Convert.ToInt32(expectedValue), Convert.ToInt32(actualValue), null,
                                new Dictionary<string, object>
                                {
                                    {Constants.ParameterNames.CheckForNonZero, false},
                                    {Constants.ParameterNames.CheckForPositive, false}
                                }, recurseProperties);

                        _asserter.AssertEquality((int?)expectedValue, (int?)actualValue,
                            additionalParameters: new Dictionary<string, object>
                        {
                            {Constants.ParameterNames.AssertDelegate, expression}
                        });
                    }
                    else if (property.PropertyType == typeof(Boolean))
                    {
                        _asserter.AssertEquality(Convert.ToBoolean(expectedValue), Convert.ToBoolean(actualValue));
                    }
                    else if (property.PropertyType == typeof(Boolean?))
                    {
                        Expression<Action<bool, bool>> expression = 
                            (e, a) => Assert.AreEqual(Convert.ToBoolean(expectedValue), Convert.ToBoolean(actualValue), null);

                        _asserter.AssertEquality((bool?)expectedValue, (bool?)actualValue,
                            additionalParameters: new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.AssertDelegate, expression}
                            });
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        _asserter.AssertEquality(Convert.ToDateTime(expectedValue), Convert.ToDateTime(actualValue), 
                            additionalParameters: new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForMinDate, false},
                                {Constants.ParameterNames.CheckForMaxDate, false}
                            });
                    }
                    else if (property.PropertyType == typeof(DateTime?))
                    {
                        Expression<Action<DateTime, DateTime>> expression = 
                            (e, a) => _asserter.AssertEquality(Convert.ToDateTime(expectedValue), Convert.ToDateTime(actualValue),
                            null, new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForMinDate, false},
                                {Constants.ParameterNames.CheckForMaxDate, false}
                            }, recurseProperties);

                        _asserter.AssertEquality((DateTime?)expectedValue, (DateTime?)actualValue,
                            additionalParameters: new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.AssertDelegate, expression}
                            });
                    }
                    else if (property.PropertyType == typeof(Int64))
                    {
                        _asserter.AssertEquality(Convert.ToInt64(expectedValue), Convert.ToInt64(actualValue),
                            additionalParameters:new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForNonZero, false},
                                {Constants.ParameterNames.CheckForPositive, false}
                            });
                    }
                    else if (property.PropertyType == typeof(Int64?))
                    {
                        Expression<Action<Int64, Int64>> expression = 
                             (e, a) =>
                                _asserter.AssertEquality(Convert.ToInt64(expectedValue), Convert.ToInt64(actualValue), 
                                null, 
                                new Dictionary<string, object>
                                {
                                    {Constants.ParameterNames.CheckForNonZero, false},
                                    {Constants.ParameterNames.CheckForPositive, false}
                                }, recurseProperties);

                        _asserter.AssertEquality((Int64?)expectedValue, (Int64?)actualValue, additionalParameters:
                            new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.AssertDelegate, expression}
                            });
                    }
                    else if (property.PropertyType == typeof(Double))
                    {
                        _asserter.AssertEquality(Convert.ToDouble(expectedValue), Convert.ToDouble(actualValue), 
                            additionalParameters:new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForNonZero, false},
                                {Constants.ParameterNames.CheckForPositive, false}
                            });
                    }
                    else if (property.PropertyType == typeof(Double?))
                    {
                        Expression<Action<Double, Double>> expression = 
                            (e, a) =>
                                _asserter.AssertEquality(Convert.ToDouble(expectedValue), Convert.ToDouble(actualValue),
                            null, new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForNonZero, false},
                                {Constants.ParameterNames.CheckForPositive, false}
                            }, recurseProperties);

                        _asserter.AssertEquality((double?)expectedValue, (double?)actualValue,
                            additionalParameters: new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.AssertDelegate, expression}
                            });
                    }
                    else if (property.PropertyType == typeof(byte))
                    {
                        _asserter.AssertEquality((byte)expectedValue, (byte)actualValue);
                    }
                    else if (property.PropertyType == typeof(Guid))   
                    {
                        _asserter.AssertEquality((Guid)expectedValue, (Guid)actualValue, 
                            additionalParameters: new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForEmptyGuid, false}
                            });
                    }
                    else if (property.PropertyType == typeof(Guid?))
                    {
                        Expression<Action<Guid, Guid>> expression = (e, a) =>
                            _asserter.AssertEquality((Guid)expectedValue, (Guid)actualValue,
                            null, new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.CheckForEmptyGuid, false}
                            }, recurseProperties);

                        _asserter.AssertEquality((Guid?)expectedValue, (Guid?)actualValue, additionalParameters:
                            new Dictionary<string, object>
                            {
                                {Constants.ParameterNames.AssertDelegate, expression}
                            });
                    }
                    else
                    {
                        HandleRecurseProperties<T>(propertiesToIgnore, additionalParameters, recurseProperties, property, expectedValue, actualValue);
                    }
                }
                catch (TargetException)
                {
                    failedProperties.Add("Property: {0} type mismatch."
                        .FormatString(property.Name));
                }
                catch (AssertException)
                {
                    failedProperties.Add("Property: {0}. Expected: {1}, Actual: {2}"
                        .FormatString(property.Name, expectedValue, actualValue));
                }
            }

            return failedProperties;
        }

        private void HandleRecurseProperties<T>(IEnumerable<string> propertiesToIgnore, IDictionary<string, object> additionalParameters,
            bool recurseProperties, PropertyInfo property, object expectedValue, object actualValue)
        {
            if (recurseProperties)
            {
                var methodInfo = _asserter.GetType().GetMethod("AssertEquality");
                var genericMethod = methodInfo.MakeGenericMethod(new[] {property.PropertyType});

                genericMethod.Invoke(_asserter, new object[]
                {
                    expectedValue, actualValue, propertiesToIgnore, additionalParameters, recurseProperties
                });

                return;
            }

            throw new NotSupportedException("Automatic assertions for Property {0} with Type {1} is not supported yet.  Manually add property to ignore list and assert manually or set recurseProperties to true."
                .FormatString(property.Name, property.PropertyType.ToString()));
        }
    }
}
