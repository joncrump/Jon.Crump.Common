using System;
using System.Collections.Generic;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Helpers;
using TightlyCurly.Com.Tests.Common.Helpers.Strategies;

namespace TightlyCurly.Com.Tests.Common.Base
{
    public abstract class TestBase : UtilityBase
    {
        protected readonly IObjectCreator ObjectCreator;
        protected readonly IAssertAdapter Assert;
        protected readonly IAsserter Asserter;

        protected ITestRunner TestRunner
        {
            get { return GetTestRunner(); }
        }

        protected TestBase(IAssertAdapter assertAdapter)
            : this(new RandomDataGenerator(), new ReflectionBasedObjectCreator(), assertAdapter, 
            new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IAssertAdapter assertAdapter)
            : this(dataGenerator, new ReflectionBasedObjectCreator(), assertAdapter, 
            new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IObjectCreator objectCreator, IAssertAdapter assertAdapter)
            : this(new RandomDataGenerator(), objectCreator, assertAdapter, new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator, IAssertAdapter assertAdapter)
            : this(dataGenerator, objectCreator, assertAdapter, new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator, IAssertAdapter assertAdapter, 
            IAsserter asserter)
            : base(dataGenerator)
        {
            ObjectCreator = objectCreator;
            Assert = assertAdapter;
            Asserter = asserter;
        }

        protected ITestRunner GetTestRunner()
        {
            return GetTestRunner(Setup, CleanUp);
        }

        protected ITestRunner GetTestRunner(Action setupAction, Action cleanupAction)
        {
            var runner = new TestRunner(Assert, setupAction, cleanupAction);

            return runner;
        }

        public void AssertIsNullOrNot<T>(T expectedValue, T actualValue, Action<T, T> assertDelegate = null)
            where T : class
        {
            if (expectedValue == null)
            {
                Assert.IsNull(actualValue);
                return;
            }

            Assert.IsNotNull(actualValue);

            if (assertDelegate != null)
            {
                assertDelegate(expectedValue, actualValue);
            }
        }

        public virtual void Setup()
        {
        }

        public virtual void CleanUp()
        {
        }

        protected void IgnoreException<TException>(Action action, string expectedMessage = null)
        {
            try
            {
                action();
            }
            catch (Exception exception)
            {
                if (exception.GetType() != typeof(TException))
                {
                    throw;
                }

                if (expectedMessage.IsNullOrEmpty())
                {
                    return;
                }

                if (String.Compare(expectedMessage, exception.Message, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return;
                }

                throw;
            }
        }

        protected void DoConstructorTests<TItem>() where TItem : class
        {
            var tester = new ConstructorTester(DataGenerator);

            tester.TestConstructorsForNullParameters<TItem>();
        }

        protected IEnumerable<T> CreateEnumerableOfItems<T>(int numberOfItems = 5) where T : class, new()
        {
            return CreateEnumerableOfItems(() => ObjectCreator.CreateNew<T>(), numberOfItems);
        }
    }

    public abstract class TestBase<TItemUnderTest> : TestBase where TItemUnderTest : class, new()
    {
        protected TItemUnderTest ItemUnderTest { get; private set; }

        protected TestBase(IAssertAdapter assertAdapter)
            : base(new RandomDataGenerator(), new ReflectionBasedObjectCreator(), assertAdapter, 
            new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IAssertAdapter assertAdapter)
            : base(dataGenerator, new ReflectionBasedObjectCreator(), assertAdapter, 
            new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IObjectCreator objectCreator, IAssertAdapter assertAdapter)
            : base(new RandomDataGenerator(), objectCreator, assertAdapter, 
            new SurrogateAsserter(assertAdapter, new AsserterStrategyFactory(assertAdapter)))
        {
        }

        protected TestBase(IDataGenerator dataGenerator, IObjectCreator objectCreator, 
            IAssertAdapter assertAdapter) : base(dataGenerator, objectCreator, assertAdapter)
        {
        }

        public override void Setup()
        {
            base.Setup();

            ItemUnderTest = new TItemUnderTest();
        }

        public override void CleanUp()
        {
            base.CleanUp();

            ItemUnderTest = null;
        }
    }
}