﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TightlyCurly.Com.Common.Data.QueryBuilders.Strategies.TSql;
using TightlyCurly.Com.Tests.Common.MsTest;

namespace TightlyCurly.Com.Common.Data.Tests.QueryBuilders.Strategies.TSql.PagedQueryBuilderStrategyTests
{
    [TestClass]
    public class TheConstructor : MsTestBase
    {
        [TestMethod]
        public void WillPassConstructorUnitTests()
        {
            DoConstructorTests<PagedQueryBuilderStrategy>();
        }
    }
}