using System;

namespace TightlyCurly.Com.Tests.Common.Base
{
    public interface ITestRunner
    {
        Action ClassSetup { get; set; }
        Action ClassCleanup { get; set; } 

        ITestRunner DoCustomSetup(Action setupDelegate);
        ITestRunner DoCustomCleanup(Action cleanupDelegate);
        ITestRunner WithTest(Action testDelegate);
        void Run(bool treatExceptionAsInconclusive = false);
        void ExecuteTest(Action test, bool treatExceptionAsInconclusive = false);
    }
}
