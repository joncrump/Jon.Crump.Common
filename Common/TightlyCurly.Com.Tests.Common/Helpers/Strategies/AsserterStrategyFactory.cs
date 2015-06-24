using System;
using System.Collections.Generic;
using System.Linq;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers.Strategies
{
    public class AsserterStrategyFactory : IAsserterStrategyFactory
    {
        private IAssertAdapter Assert { get; set; }
        private Dictionary<string, Tuple<Type, IAsserterStrategy>> _strategies;
        public IAsserter Asserter { get; set; }

        public AsserterStrategyFactory(IAssertAdapter assertAdapter)
        {
            Assert = assertAdapter;
            InitializePrimitiveStrategies();
        }

        public IAsserterStrategy GetStrategy<TValue>()
        {
            var strategy = GetPrimitiveStrategy<TValue>();

            return strategy.IsNotNull()
                ? strategy
                : GetGenericStrategyBasedOnType<TValue>();
        }

        private IAsserterStrategy GetGenericStrategyBasedOnType<TValue>()
        {
            var type = typeof(TValue);

            var isEnumerable = type.IsGenericType && type.GetInterface("IEnumerable").IsNotNull();

            if (isEnumerable)
            {
                return GetGenericEnumerableStrategy<TValue>();
            }

            var isNullable = type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Nullable<>);

            if (isNullable)
            {
                return GetNullableStrategy<TValue>();
            }

            var isTuple2 = type.IsGenericType
                && type.GetGenericTypeDefinition() == typeof(Tuple<,>);

            if (isTuple2)
            {
                return GetTuple2Strategy<TValue>();
            }

            var isEnumType = type.BaseType == typeof(Enum);

            if (isEnumType)
            {
                return GetEnumStrategy();
            }

            return GetObjectStrategy<TValue>();
        }

        private IAsserterStrategy GetEnumStrategy()
        {
            var enumStrategyType = typeof(EnumAsserterStrategy);

            var strategy = (IAsserterStrategy)Activator.CreateInstance(enumStrategyType,
                new object[] { Assert });

            return strategy;
        }

        private IAsserterStrategy GetNullableStrategy<TValue>()
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(TValue));
            var nullableStrategyType = typeof(NullableAsserterStrategy<>);
            var genericNullableStrategyType = nullableStrategyType.MakeGenericType(new[] { underlyingType });

            var strategy = (IAsserterStrategy)Activator.CreateInstance(genericNullableStrategyType,
                new object[] { Assert });

            return strategy;
        }

        private IAsserterStrategy GetObjectStrategy<TValue>()
        {
            var objectStrategyType = typeof(ObjectAsserterStrategy<>);
            var genericObjectStrategyType = objectStrategyType.MakeGenericType(new[] { typeof(TValue) });

            var strategy = (IAsserterStrategy)Activator.CreateInstance(genericObjectStrategyType,
                new object[] { Assert, Asserter });

            return strategy;
        }

        private IAsserterStrategy GetTuple2Strategy<TValue>()
        {
            var tupleType = typeof(TValue);

            var item1Type = tupleType.GetGenericTypeDefinition()
                .GetGenericArguments()
                .First();
            var item2Type = tupleType.GetGenericTypeDefinition().GetGenericArguments()
                .Take(2).Last();

            var tupleStrategyType = typeof(Tuple2AsserterStrategy<,>);
            var genericTupleStrategyType = tupleStrategyType.MakeGenericType(
                new[] { item1Type, item2Type });

            var strategy = (IAsserterStrategy)Activator.CreateInstance(genericTupleStrategyType,
                new object[] { Assert });

            return strategy;
        }

        private IAsserterStrategy GetGenericEnumerableStrategy<TValue>()
        {
            var enumerableStrategyType = typeof(EnumerableAsserterStrategy<>);

            var valueType = typeof(TValue);
            var argumentTypes = valueType.IsGenericType
                ? valueType.GetGenericArguments().ToArray()
                : new[] { valueType };

            if (argumentTypes.Count() == 2
                && valueType.GetGenericTypeDefinition() == typeof(IDictionary<,>))
            {
                var keyValuePairType = typeof(KeyValuePair<,>);
                keyValuePairType = keyValuePairType.MakeGenericType(argumentTypes);

                argumentTypes = new[] { keyValuePairType };
            }

            var genericEnumerableStrategyType = enumerableStrategyType.MakeGenericType(argumentTypes);
            var factory = this;

            var strategy = (IAsserterStrategy)Activator.CreateInstance(genericEnumerableStrategyType,
                new object[] { Assert, factory });

            return strategy;
        }

        private IAsserterStrategy GetPrimitiveStrategy<TValue>()
        {
            IAsserterStrategy strategy = null;

            var tuple = _strategies
                .Values
                .FirstOrDefault(t => t.Item1 == typeof(TValue));

            if (tuple.IsNotNull())
            {
                strategy = tuple.Item2;
            }

            return strategy;
        }

        private void InitializePrimitiveStrategies()
        {
            _strategies = new Dictionary<string, Tuple<Type, IAsserterStrategy>>
                {
                    {
                        Constants.StrategyNames.ByteAsserterStrategyName, 
                        new Tuple<Type, IAsserterStrategy>(typeof(byte), new ByteAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.DateTimeAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(DateTime), new DateTimeAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.DoubleAsserterStrategyName,
                        new Tuple<Type, IAsserterStrategy>(typeof(Double), new DoubleAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.EnumAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(Enum), new EnumAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.GuidAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(Guid), new GuidAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.Int32AsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(Int32), new Int32AsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.LongAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(Int64), new LongAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.StringAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(String), new StringAsserterStategy(Assert))
                    },
                    {
                        Constants.StrategyNames.UriAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(Uri), new UriAsserterStrategy(Assert))
                    },
                    {
                        Constants.StrategyNames.BooleanAsserterStrategy,
                        new Tuple<Type, IAsserterStrategy>(typeof(Boolean), new BooleanAsserterStrategy(Assert))
                    }
                };
        }
    }
}
