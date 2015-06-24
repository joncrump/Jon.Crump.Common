using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.Attributes.JoinAttributeTests
{
    [TestClass]
    public class TheChildPropertyProperty : MsTestBase
    {
        [TestMethod]
        public void WillReturnRightKeyIfChildPropertyPassedToConstructorIsNull()
        {
            var expected = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                        DataGenerator.GenerateString(), expected);

                    Assert.AreEqual(expected, attribute.RightKey);
                    Assert.AreEqual(expected, attribute.ChildProperty);
                });
        }

        [TestMethod]
        public void WillReturnRightKeyIfChildPropertyPassedToConstructorIsEmpty()
        {
            var expected = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                        DataGenerator.GenerateString(), expected, childProperty: String.Empty);

                    Asserter.AssertEquality(expected, attribute.RightKey);
                    Asserter.AssertEquality(expected, attribute.ChildProperty);
                });
        }

        [TestMethod]
        public void WillReturnChildPropertyIfChildPropertyPassedToConstructorHasValue()
        {
            var expected = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expected = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var attribute = new JoinAttribute(JoinType.Left, typeof(object),
                        DataGenerator.GenerateString(), DataGenerator.GenerateString(),
                        childProperty: expected);

                    Assert.AreNotEqual(expected, attribute.RightKey);
                    Asserter.AssertEquality(expected, attribute.ChildProperty);
                });
        }
    }
}
