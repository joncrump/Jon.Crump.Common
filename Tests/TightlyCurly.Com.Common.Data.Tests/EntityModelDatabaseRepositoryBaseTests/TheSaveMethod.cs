using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Tests.Common.MsTest.Data;

namespace TightlyCurly.Com.Common.Data.Tests.EntityModelDatabaseRepositoryBaseTests
{
    [TestClass]
    public class TheSaveMethod : MsTestMoqRepositoryBase<TestableEntityDatabaseRepository>
    {
        [TestMethod]
        public void WillThrowArgumentNullExceptionIfModelIsNull()
        {
            TestRunner.ExecuteTest(() =>
            {
                Asserter
                    .AssertExceptionIsThrown<ArgumentNullException>(
                        () => ItemUnderTest.Save(null, true))
                    .AndVerifyHasParameter("model");
            });
        }
    }
}
