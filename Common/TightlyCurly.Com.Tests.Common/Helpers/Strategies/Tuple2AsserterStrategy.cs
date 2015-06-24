using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class Tuple2AsserterStrategy<TItem1, TItem2> : AsserterStrategyBase<Tuple<TItem1, TItem2>>
    {
        public override string Name
        {
            get { return Constants.StrategyNames.Tuple2AsserterStrategy; }
        }

        public override Type Type
        {
            get { return typeof(Tuple<TItem1, TItem2>); }
        }

        public Tuple2AsserterStrategy(IAssertAdapter assertAdapter) : base(assertAdapter)
        {
        }

        public override void AssertEquality(Tuple<TItem1, TItem2> expected, Tuple<TItem1, TItem2> actual, 
            IEnumerable<string> propertiesToIgnore = null, IDictionary<string, object> additionalParameters = null, 
            bool recurseProperties = false)
        {
            var checkForDefault = ExtractParameter<bool>(additionalParameters, Constants.ParameterNames.CheckForDefault);
            var item1AssertDelegate = ExtractParameter<Expression<Action<TItem1, TItem1>>>(additionalParameters, Constants.ParameterNames.Item1AssertDelegate);
            var item2AssertDelegate = ExtractParameter<Expression<Action<TItem2, TItem2>>>(additionalParameters, Constants.ParameterNames.Item2AssertDelegate);

            if (checkForDefault)
            {
                CheckForDefault(actual);
            }

            if (item1AssertDelegate.IsNotNull())
            {
                item1AssertDelegate.Compile()(expected.Item1, actual.Item1);
            }

            if (item2AssertDelegate.IsNotNull())
            {
                item2AssertDelegate.Compile()(expected.Item2, actual.Item2);
            }
        }
    }
}
