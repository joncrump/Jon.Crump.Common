using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.Attributes.JoinAttributeTests
{
    [TestClass]
    public class TheParentPropertyProperty : MsTestBase
    {
        [TestMethod]
        public void WillReturnLeftKeyIfParentPropertyPassedToConstructorIsNull()
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
                        expected, DataGenerator.GenerateString());

                    Assert.AreEqual(expected, attribute.LeftKey);
                    Assert.AreEqual(expected, attribute.ParentProperty);
                });
        }

        [TestMethod]
        public void WillReturnLeftKeyIfParentPropertyPassedToConstructorIsEmpty()
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
                        expected, DataGenerator.GenerateString(), parentProperty:String.Empty);

                    Asserter.AssertEquality(expected, attribute.LeftKey);
                    Asserter.AssertEquality(expected, attribute.ParentProperty);
                });
        }

        [TestMethod]
        public void WillReturnParentPropertyIfParentPropertyPassedToConstructorHasValue()
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
                        parentProperty: expected);

                    Assert.AreNotEqual(expected, attribute.LeftKey);
                    Asserter.AssertEquality(expected, attribute.ParentProperty);
                });
        }
    }
}