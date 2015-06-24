using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Helpers;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Tests.Helpers.HashHelperTests
{
    [TestClass]
    public class TheGenerateHashMethod : MsTestMoqTestBase<HashHelper>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfValueIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.GenerateHash(null))
                    .AndVerifyHasParameter("value");
            });
        }

        [TestMethod]
        public void WillReturnHashIfValueIsHashable()
        {
            TestRunner
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GenerateHash(new HashableTestClass());

                    Asserter.AssertEquality("TestingHash", actual);
                });
        }

        [TestMethod]
        public void WillReturnHashCodeOfObject()
        {
            string value = null;
            string expected = null;

            TestRunner
                .DoCustomSetup(() =>
                {
                    value = DataGenerator.GenerateString();

                    expected = value.GetHashCode().ToString();
                })
                .ExecuteTest(() =>
                {
                    var actual = ItemUnderTest.GenerateHash(value);

                    Asserter.AssertEquality(expected, actual);
                });
        }

        public class HashableTestClass : IHashable
        {
            public string Hash
            {
                get { return "TestingHash"; }
            }
        }
    }
}
