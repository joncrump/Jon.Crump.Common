using System;
using System.Collections.Generic;
using System.Linq;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Helpers.Strategies;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public class SurrogateAsserter : IAsserter
    {
        private IAssertAdapter Assert { get; set; }
        private readonly IAsserterStrategyFactory _asserterStrategyFactory;

        public SurrogateAsserter(IAssertAdapter asserterAdapter)
            : this(asserterAdapter, new AsserterStrategyFactory(asserterAdapter))
        {
        }

        public SurrogateAsserter(IAssertAdapter assertAdapter, IAsserterStrategyFactory asserterStrategyFactory)
        {
            Assert = assertAdapter;
            _asserterStrategyFactory = asserterStrategyFactory;

            _asserterStrategyFactory.Asserter = this;
        }
        
        public void AssertEquality<TValue>(TValue expected, TValue actual, IEnumerable<string> propertiesToIgnore = null,
            IDictionary<string, object> additionalParameters = null, bool recurseObjectProperties = true)
        {
            var strategy = _asserterStrategyFactory.GetStrategy<TValue>();

            strategy.AssertEquality(expected, actual, propertiesToIgnore, additionalParameters, 
                recurseObjectProperties);
        }

        public void AssertHasClassAttributes<TValue>(TValue value, IEnumerable<AttributeInfo> attributeInfos)
        {
            if (attributeInfos.IsNullOrEmpty())
            {
                return;
            }

            var valueType = typeof(TValue);
            var attributes = valueType.GetCustomAttributes(true)
                .SafeSelect(a => (Attribute)a);

            if (attributes.IsNullOrEmpty())
            {
                throw new AssertException("Could not locate any attributes on type {0}.  Expected the following attribute(s):\r\n{1}"
                    .FormatString(valueType.FullName, String.Join("\r\n", attributeInfos.Select(a => a.AttributeType.FullName))));
            }

            AssertAttributes(attributeInfos, attributes, valueType.FullName);
        }

        public void AssertHasPropertyAttributes<TValue>(TValue value, IEnumerable<PropertyAttributeInfo> attributeInfos)
        {
            if (attributeInfos.IsNull())
            {
                return;
            }

            var valueType = typeof(TValue);
            var properties = valueType.GetProperties();
            
            foreach (var property in properties)
            {
                var property1 = property;
                var propertyAttributeInfos = attributeInfos
                    .Where(p => p.Property == property1.Name);

                if (!propertyAttributeInfos.IsNotNullOrEmpty())
                {
                    continue;
                }

                var attributes = property.GetCustomAttributes(true)
                    .SafeSelect(a => (Attribute)a);

                if (attributes.IsNullOrEmpty())
                {
                    throw new AssertException("Could not locate any attributes on property {0} for Type {1}.  Expected the following attribute(s):\r\n{2}"
                        .FormatString(property.Name, valueType.FullName, String.Join("\r\n", attributeInfos.Select(a => a.AttributeType.FullName))));
                }

                AssertAttributes(propertyAttributeInfos, attributes, "{0}.{1}"
                    .FormatString(valueType.FullName, property.Name));
            }  
        }

        public IExceptionAsserter AssertExceptionIsThrown<TException>(Action exceptionCallback) where TException : Exception
        {
            var exceptionAsserter = new ExceptionAsserter(Assert);

            exceptionAsserter.AssertExceptionIsThrown<TException>(exceptionCallback);

            return exceptionAsserter;
        }

        private void AssertAttributes(IEnumerable<AttributeInfo> attributeInfos,
            IEnumerable<Attribute> attributes, string selectorName)
        {
            foreach (var attributeInfo in attributeInfos)
            {
                var attribute = attributes
                    .FirstOrDefault(a => a.GetType().FullName == attributeInfo.AttributeType.FullName);

                if (attribute.IsNull())
                {
                    throw new AssertException("Expected attribute {0} was not found in {1}"
                        .FormatString(attributeInfo.AttributeType.FullName, selectorName));
                }

                AssertAttribute(attribute, attributeInfo);
            }
        }

        private void AssertAttribute(Attribute attribute, AttributeInfo attributeInfo)
        {
            foreach (var keyValuePair in attributeInfo.AttributeValues)
            {
                var attributeProperty = attribute.GetType().GetProperty(keyValuePair.Key);

                if (attributeProperty.IsNull())
                {
                    throw new AssertException("Expected property {0} on attribute [1}.  Property was not found"
                        .FormatString(attributeProperty.Name, attribute.GetType().FullName));
                }

                Assert.AreEqual(attributeProperty.GetValue(attribute), keyValuePair.Value);
            }
        }
    }
}