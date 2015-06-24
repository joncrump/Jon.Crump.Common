using System;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public interface IExceptionAsserter
    {
        Exception CaughtException { get; }
        IExceptionAsserter AssertExceptionIsThrown<TException>(Action exceptionCallback) where TException : Exception;
        IExceptionAsserter AndVerifyMessageContains(string message);
        IExceptionAsserter AndVerifyHasParameter(string parameterName);
    }
}
