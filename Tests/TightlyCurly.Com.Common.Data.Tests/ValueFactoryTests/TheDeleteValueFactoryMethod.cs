using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Data.Constants;
using TightlyCurly.Com.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.ValueFactoryTests
{
    [TestClass]
    public class TheDeleteValueFactoryMethod : MsTestMoqTestBase<TestableValueFactory>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfKeyIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.DeleteValueFactory(null))
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
                        () => ItemUnderTest.DeleteValueFactory(String.Empty))
                    .AndVerifyHasParameter("key");
            });
        }

        [TestMethod]
        public void WillThrowInvalidOperationExceptionIfKeyDoesNotExist()
        {
            TestRunner
                .DoCustomSetup(() =>
                {
                    Expression<Func<object, object>> expression = e => null;

                    ItemUnderTest.Delegates.Add(DataGenerator.GenerateString(), expression);
                })
                .ExecuteTest(() =>
                {
                   Asserter
                        .AssertExceptionIsThrown<InvalidOperationException>(
                            () => ItemUnderTest.DeleteValueFactory(DataGenerator.GenerateString()))
                        .AndVerifyMessageContains(ErrorMessages.CannotDeleteExpression);
                });
        }

        [TestMethod]
        public void WillDeleteValueFactory()
        {
            var key = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    key = DataGenerator.GenerateString();
                    Expression<Func<object, object>> expression = e => null;

                    ItemUnderTest.Delegates.Add(key, expression);
                })
                .ExecuteTest(() =>
                {
                    ItemUnderTest.DeleteValueFactory(key);

                    Assert.IsFalse(ItemUnderTest.Delegates.ContainsKey(key));
                });
        }
    }
}