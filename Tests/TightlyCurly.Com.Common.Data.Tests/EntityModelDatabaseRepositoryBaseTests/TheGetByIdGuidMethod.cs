using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TightlyCurly.Com.Common.Exceptions;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheGetByIdGuidMethod : MsTestMoqRepositoryBase<TestableEntityDatabaseRepository>
    {
        [TestMethod]
        public void WillThrowArgumentInvalidExceptionIfIdIsEmpty()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentInvalidException>(
                        () => ItemUnderTest.GetById(Guid.Empty))
                    .AndVerifyHasParameter("id");
            });
        }
    }
}