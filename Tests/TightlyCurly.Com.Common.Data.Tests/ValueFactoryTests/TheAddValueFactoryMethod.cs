using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.Constants;
using TightlyCurly.Com.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.ValueFactoryTests
{
    [TestClass]
    public class TheAddValueFactoryMethod : MsTestMoqTestBase<TestableValueFactory>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfKeyIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.AddValueFactory(null, e => new object()))
                    .AndVerifyHasParameter("key");
            });
        }

        [TestMethod]
        public void WillThrowArgumentEmptyExceptionIfKeyIsEmpty()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentEmptyException>(
                        () => ItemUnderTest.AddValueFactory(String.Empty, e => new object()))
                    .AndVerifyHasParameter("key");
            });
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfValueFactoryIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.AddValueFactory(DataGenerator.GenerateString(), (Expression<Action>) null))
                    .AndVerifyHasParameter("valueFactory");
            });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfValueFactoryAlreadyExists()
        {
            var key = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    Expression<Func<object, object>> expression = e => null;
                    key = DataGenerator.GenerateString();

                    ItemUnderTest.Delegates.Add(key, expression);
                })
                .ExecuteTest(() =>
                {
                    Asserter
                        .AssertExceptionIsThrown<InvalidOperationException>(
                            () => ItemUnderTest.AddValueFactory(key, e => (Expression<Action>)null))
                        .AndVerifyMessageContains(ErrorMessages.CannotAddExpression);
                });
        }

        [TestMethod]
        public void WillAddDelegate()
        {
            var key = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    key = DataGenerator.GenerateString();
                    Expression<Func<object, object>> expression = e => null;

                    ItemUnderTest.Delegates.Add(DataGenerator.GenerateString(), expression);
                })
                .ExecuteTest(() =>
                {
                    Expression<Func<object, object>> param = e => DataGenerator.GenerateInteger(1, 5);

                    ItemUnderTest.AddValueFactory(key, param);

                    Assert.IsTrue(ItemUnderTest.Delegates.ContainsKey(key));
                });
        }
    }
}
