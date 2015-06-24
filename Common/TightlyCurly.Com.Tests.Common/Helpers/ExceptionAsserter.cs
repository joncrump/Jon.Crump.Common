using System;
using TightlyCurly.Com.Common;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Tests.Common.Helpers
{
    public class ExceptionAsserter : IExceptionAsserter
    {
// ReSharper disable once InconsistentNaming
        private readonly IAssertAdapter Assert;

        public Exception CaughtException { get; private set; }

        public ExceptionAsserter(IAssertAdapter assertAdapter)
        {
            Assert = assertAdapter;
        }

        public IExceptionAsserter AssertExceptionIsThrown<T>(Action exceptionCallback)
            where T : Exception
        {
            var called = false;
            Exception thrownException = null;

            try
            {
                exceptionCallback();
            }
            catch (Exception exception)
            {
                thrownException = exception;

                if (thrownException.GetType() != typeof(T))
                {
                    throw;
                }

                called = true;
                CaughtException = exception;

                return this;
            }
            finally
            {
                if (!called)
                {
                    if (CaughtException.IsNull())
                    {
                        throw new AssertException("Expected exception type: {0}. No exception was thrown.".FormatString(typeof(T)));
                    }

                    throw new AssertException("Expected exception type: {0}.  Received type {1} instead.".FormatString(typeof(T), CaughtException.GetType()));
                }
            }

            return null;
        }

        public IExceptionAsserter AndVerifyMessageContains(string message)
        {
            if (CaughtException.IsNull())
            {
                throw new InvalidOperationException("The caught exception cannot be null.");
            }

            Guard.EnsureIsNotNullOrEmpty("message", message);

            Assert.IsTrue(CaughtException.Message.IsNotNullOrEmpty(), "The exception message is null or empty.");
            Assert.IsTrue(CaughtException.Message.Contains(message), "The exception message did not contain the expected message.\r\n.Expected: {0}\r\n.Actual: {1}".FormatString(
                message, CaughtException.Message));

            return this;
        }

        public IExceptionAsserter AndVerifyHasParameter(string parameterName)
        {
            if (CaughtException.IsNull())
            {
                throw new InvalidOperationException("The caught exception cannot be null.");
            }

            var argumentException = CaughtException as ArgumentException;

            if (argumentException.IsNull())
            {
                throw new InvalidOperationException("The caught exception must be a form of ArgumentException.");
            }

            Guard.EnsureIsNotNullOrEmpty("parameterName", parameterName);

            Assert.AreEqual(parameterName, argumentException.ParamName, 
                "Expected parameterName {0} was not found.  The exception contained parameterName {1} instead.".FormatString(parameterName, 
                argumentException.ParamName));

            return this;
        }
    }
}
