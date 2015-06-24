using System;
using TightlyCurly.Com.Common.Extensions;
using TightlyCurly.Com.Tests.Common.Helpers;

namespace TightlyCurly.Com.Tests.Common.Base
{
    public class TestRunner : ITestRunner
    {
        public Action ClassSetup { get; set; }
        public Action ClassCleanup { get; set; } 

        private Action _additionalSetup;
        private Action _additionalCleanup;
        private Action _test;

        public IAssertAdapter Assert { get; set; }

        public TestRunner(IAssertAdapter assertAdapter, Action classSetup, Action classCleanup)
        {
            Assert = assertAdapter;
            ClassSetup = classSetup;
            ClassCleanup = classCleanup;
        }

        public void Run(bool treatExceptionAsInconclusive = false)
        {
            if (ClassSetup.IsNotNull())
            {
                ClassSetup();
            }

            if (_additionalSetup != null)
            {
                _additionalSetup();
            }

            try
            {
                _test();
            }
            catch (Exception exception)
            {
                if (treatExceptionAsInconclusive)
                {
                    Assert.Inconclusive(String.Format("An exception was thrown during the test execution:\n\n{0}\n\nStackTrace:{1}",
                                      exception.Message, exception.StackTrace));
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if (_additionalCleanup.IsNotNull())
                {
                    _additionalCleanup();
                }

                if (ClassCleanup.IsNotNull())
                {
                    ClassCleanup();
                }
            }
        }

        public ITestRunner DoCustomSetup(Action setupDelegate)
        {
            _additionalSetup = setupDelegate;

            return this;
        }

        public ITestRunner DoCustomCleanup(Action cleanupDelegate)
        {
            _additionalCleanup = cleanupDelegate;

            return this;
        }

        public ITestRunner WithTest(Action test)
        {
            _test = test;

            return this;
        }

        public void ExecuteTest(Action test, bool treatExceptionAsInconclusive = false)
        {
            WithTest(test);
            Run(treatExceptionAsInconclusive);
        }
    }
}
