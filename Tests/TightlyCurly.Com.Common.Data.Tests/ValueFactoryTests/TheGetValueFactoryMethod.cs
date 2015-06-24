using System;
using System.Linq.Expressions;
using TightlyCurly.Com.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.ValueFactoryTests
{
    [TestClass]
    public class TheGetValueFactoryMethod : MsTestMoqTestBase<TestableValueFactory>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfKeyIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.GetValueFactory(null))
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
                        () => ItemUnderTest.GetValueFactory(String.Empty))
                    .AndVerifyHasParameter("key");
            });
        }

        [TestMethod]
        public void WillReturnExpressionIfParameterIsNull()
        {
            Expression<Func<object, object>> expected = null;
            var key = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = e => null;
                    key = DataGenerator.GenerateString();

                    ItemUnderTest.Delegates
                        .Add(key, expected);
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GetValueFactory(key);

                    Asserter.AssertEquality("() => e => null", actual.ToString());
                });
        }

        [TestMethod]
        public void WillReturnExpressionWithParameter()
        {
            string expected = null;
            var key = String.Empty;
            var random = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    key = DataGenerator.GenerateString();
                    expected = DataGenerator.GenerateString();
                    random = DataGenerator.GenerateString();

                    Expression<Func<object, string>> expression = e => e.ToString() + random;

                    ItemUnderTest.Delegates
                        .Add(key, expression);
                })
                .ExecuteTest(() =>
                {
                    var returnExpression = ItemUnderTest.GetValueFactory(key, new ParameterInfo(typeof(string), expected));
                    var actual = returnExpression.Compile()();

                    Asserter.AssertEquality(expected + random, actual.ToString());
                });
        }
    }
}