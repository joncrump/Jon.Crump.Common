using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheGetByIdModelMethod : MsTestMoqRepositoryBase<TestableEntityDatabaseRepository>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.GetById(null))
                    .AndVerifyHasParameter("model");
            });
        }
    }
}
