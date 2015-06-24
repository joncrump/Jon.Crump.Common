using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Data.Attributes;
using TightlyCurly.Com.Common.Exceptions;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.Attributes.JoinAttributeTests
{
    [TestClass]
    public class TheConstructor : MsTestBase
    {
        [TestMethod]
        public void WillThrowArgumentInvalidExceptionIfJoinTypeIsNone()
        {
            Asserter
                .AssertExceptionIsThrown<ArgumentInvalidException>(
                    () => new JoinAttribute(JoinType.None, It.IsAny<Type>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("relationshipType");
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfJoinedTypeIsNull()
        {
            Asserter
                .AssertExceptionIsThrown<ArgumentNullException>(
                    () => new JoinAttribute(JoinType.Inner, It.IsAny<Type>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("joinedType");
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfLeftKeyIsNull()
        {
            Asserter
                .AssertExceptionIsThrown<ArgumentNullException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), null,
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("leftKey");
        }

        [TestMethod]
        public void WillThrowArgumentEmptyExceptionIfLeftKeyIsEmpty()
        {
            Asserter
                .AssertExceptionIsThrown<ArgumentEmptyException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), String.Empty,
                        It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("leftKey");
        }

        [TestMethod]
        public void WillThrowArgumentNullExceptionIfRightKeyIsNull()
        {
            Asserter
                .AssertExceptionIsThrown<ArgumentNullException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), DataGenerator.GenerateString(),
                        null, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("rightKey");
        }

        [TestMethod]
        public void WillThrowArgumentEmptyExceptionIfRightKeyIsEmpty()
        {
            Asserter
                .AssertExceptionIsThrown<ArgumentEmptyException>(
                    () => new JoinAttribute(JoinType.Inner, typeof(object), 
                        DataGenerator.GenerateEmailAddress(),
                        String.Empty, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<string>(), It.IsAny<JoinType>(), It.IsAny<string>(),
                        It.IsAny<string>()))
                .AndVerifyHasParameter("rightKey");
        }

        [TestMethod]
        public void WillPopulateProperties()
        {
            var expectedJoinType = JoinType.None;
            Type expectedType = null;
            var expectedLeftKey = String.Empty;
            var expectedRightKey = String.Empty;
            var expectedJoinTable = String.Empty;
            var expectedJoinTableLeftKey = String.Empty;
            var expectedJoinTableRightKey = String.Empty;
            var expectedJoinTableJoinType = JoinType.None;
            var expectedParentProperty = String.Empty;
            var expectedChildProperty = String.Empty;

            TestRunner
                .DoCustomSetup(() =>
                {
                    expectedJoinType = JoinType.Left;
                    expectedType = typeof(Object);
                    expectedLeftKey = DataGenerator.GenerateString();
                    expectedRightKey = DataGenerator.GenerateString();
                    expectedJoinTable = DataGenerator.GenerateString();
                    expectedJoinTableLeftKey = DataGenerator.GenerateString();
                    expectedJoinTableRightKey = DataGenerator.GenerateString();
                    expectedJoinTableJoinType = JoinType.Right;
                    expectedParentProperty = DataGenerator.GenerateString();
                    expectedChildProperty = DataGenerator.GenerateString();
                })
                .ExecuteTest(() =>
                {
                    var join = new JoinAttribute(expectedJoinType, expectedType, expectedLeftKey,
                        expectedRightKey, expectedJoinTable, expectedJoinTableLeftKey,
                        expectedJoinTableRightKey, expectedJoinTableJoinType,
                        expectedParentProperty, expectedChildProperty);

                    Asserter.AssertEquality(expectedJoinType, join.JoinType);
                    Asserter.AssertEquality(expectedType.ToString(), join.JoinedType.ToString());
                    Asserter.AssertEquality(expectedLeftKey, expectedLeftKey);
                    Asserter.AssertEquality(expectedRightKey, join.RightKey);
                    Asserter.AssertEquality(expectedJoinTable, join.JoinTable);
                    Asserter.AssertEquality(expectedJoinTableLeftKey, join.JoinTableLeftKey);
                    Asserter.AssertEquality(expectedJoinTableRightKey, join.JoinTableRightKey);
                    Asserter.AssertEquality(expectedJoinTableJoinType, join.JoinTableJoinType);
                    Asserter.AssertEquality(expectedParentProperty, join.ParentProperty);
                    Asserter.AssertEquality(expectedChildProperty, join.ChildProperty);
                });
        }
    }
}