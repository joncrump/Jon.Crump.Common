using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class EnumerableAsserterStrategy<TModel> : AsserterStrategyBase<IEnumerable<TModel>>
    {
        private readonly IAsserterStrategyFactory _asserterStrategyFactory;

        public EnumerableAsserterStrategy(IAssertAdapter assertAdapter, 
            IAsserterStrategyFactory asserterStrategyFactory)
            : base(assertAdapter)
        {
            _asserterStrategyFactory = Guard.EnsureIsNotNull("asserterStrategyFactory", asserterStrategyFactory);
        }

        public override string Name
        {
            get { return Constants.StrategyNames.EnumerableAsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(IEnumerable<TModel>); }
        }

        public override void AssertEquality(IEnumerable<TModel> expected, 
            IEnumerable<TModel> actual, 
            IEnumerable<string> propertiesToIgnore = null,
            IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);
            var allowNullOrEmpty = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.AllowNullOrEmpty);
            var comparisonDelegate = ExtractParameter<Expression<Action<TModel, TModel>>>(additionalParameters, Constants.ParameterNames.ComparisonDelegate);
    
            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            if (!allowNullOrEmpty)
            {
                Assert.IsFalse(actual.IsNullOrEmpty(), "Actual is null or empty.  Null or empty collections are not allowed.");
            }

            if (expected.IsNullOrEmpty())
            {
                Assert.IsTrue(actual.IsNullOrEmpty());
                return;
            }

            Assert.AreEqual(expected.Count(), actual.Count());

            TModel[] tempExpected;
            TModel[] tempActual;

            SortValues(expected, actual, out tempExpected, out tempActual);

            for (var index = 0; index < tempExpected.Length; index++)
            {
                var expectedValue = tempExpected[index];
                var actualValue = tempActual[index];

                var comparisonMethod = GetComparisonMethod(comparisonDelegate, propertiesToIgnore, 
                    additionalParameters, recurseProperties)
                    .Compile();

                comparisonMethod(expectedValue, actualValue);
            }
        }

        private Expression<Action<TValue, TValue>> GetComparisonMethod<TValue>(
            Expression<Action<TValue, TValue>> comparisonDelegate, 
            IEnumerable<string> propertiesToIgnore, 
            IDictionary<string, object> additionalParameters, bool recurseProperties)
        {
            if (comparisonDelegate.IsNotNull())
            {
                return comparisonDelegate;
            }

            var strategy = _asserterStrategyFactory.GetStrategy<TValue>();

            Expression<Action<TValue, TValue>> assertDelegate = 
                (e, a) => strategy.AssertEquality(e, a, propertiesToIgnore, 
                additionalParameters, recurseProperties);

            return assertDelegate;
        }

        private void SortValues(IEnumerable<TModel> expected, IEnumerable<TModel> actual, out TModel[] tempExpected, out TModel[] tempActual)
        {
            var sortKeyProperty = GetSortPropertyFromModel<TModel>();

            if (sortKeyProperty.IsNotNull())
            {
                var parameterExpression = Expression.Parameter(typeof (TModel), "m");
                var propertyExpression = Expression.Property(parameterExpression, sortKeyProperty);
                var delegateType = typeof (Func<,>).MakeGenericType(typeof (TModel), sortKeyProperty.PropertyType);
                var lambda = Expression.Lambda(delegateType, propertyExpression, parameterExpression);

                var methodInfo = typeof (Queryable)
                    .GetMethods()
                    .Single(method => method.Name == "OrderBy"
                                                   && method.IsGenericMethodDefinition
                                                   && method.GetGenericArguments().Length == 2
                                                   && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof (TModel), sortKeyProperty.PropertyType);
                    
                 tempExpected = ((IOrderedQueryable<TModel>)methodInfo
                    .Invoke(null, new object[] { expected, lambda }))
                    .ToArray();

                tempActual = ((IOrderedQueryable<TModel>)methodInfo
                    .Invoke(null, new object[] { actual, lambda }))
                    .ToArray();
            }
            else
            {
                tempExpected = expected.ToArray();
                tempActual = actual.ToArray();
            }
        }

        private PropertyInfo GetSortPropertyFromModel<TValue>()
        {
            return typeof (TValue)
                .GetProperties()
                .FirstOrDefault(propertyInfo => propertyInfo
                    .GetCustomAttributes()
                    .Any(a => a.GetType() == typeof (SortKeyAttribute)));
        }
    }
}
