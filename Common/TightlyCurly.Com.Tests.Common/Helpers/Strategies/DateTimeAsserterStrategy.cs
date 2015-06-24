using System;
using System.Collections.Generic;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class DateTimeAsserterStrategy : AsserterStrategyBase<DateTime>
    {
        public override string Name
        {
            get
            {
                return Constants.StrategyNames.DateTimeAsserterStrategy;
            }
        }

        public override Type Type
        {
            get { return typeof(DateTime); }
        }

        public DateTimeAsserterStrategy(IAssertAdapter assertAdapter)
            : base(assertAdapter)
        {
        }

        public override void AssertEquality(DateTime expected, DateTime actual, IEnumerable<string> propertiesToIgnore = null,
            IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);
            var checkForMinDate = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForMinDate);
            var checkForMaxDate = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForMaxDate);
            var toleranceInMilliseconds = ExtractParameter<int>(additionalParameters, Constants.ParameterNames.ToleranceInMilliseconds);

            if (toleranceInMilliseconds == 0)
            {
                toleranceInMilliseconds = 5000;
            }

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            if (checkForMinDate)
            {
                Assert.IsTrue(actual > DateTime.MinValue);
            }

            if (checkForMaxDate)
            {
                Assert.IsTrue(actual < DateTime.MaxValue);
            }

            var actualDeviance = Math.Abs(expected.Subtract(actual).Milliseconds);

            if (actualDeviance > toleranceInMilliseconds
                || expected.DayOfYear != actual.DayOfYear
                || expected.Year != actual.Year)
            {
                throw new AssertException("Dates are not equal.  Expected: {0}, Actual: {1}".FormatString(expected, actual));
            }
        }
    }
}
