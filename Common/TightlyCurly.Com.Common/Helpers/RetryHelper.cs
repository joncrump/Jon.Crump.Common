using System;
using TightlyCurly.Com.Common.Extensions;

namespace TightlyCurly.Com.Common.Helpers
{
    public class RetryHelper : IRetryHelper
    {
        public event ExceptionOccurredHandler ExceptionOccurred;

        public void Retry(Action action, RetryPolicy retryPolicy = RetryPolicy.ThrowException, int numberOfRetries = 3)
        {
            Guard.EnsureIsNotNull("action", action);
            Guard.EnsureIsValid("retryPolicy", r => r > 0, retryPolicy);
            Guard.EnsureIsValid("numberOfRetries", i => i > 0, numberOfRetries);

            var numberOfExecutions = 0;
            Exception caughtException = null;

            while (numberOfExecutions < numberOfRetries)
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    if (numberOfExecutions != (numberOfRetries - 1))
                    {
                        continue;
                    }

                    if (retryPolicy == RetryPolicy.ThrowException)
                    {
                        throw;
                    }

                    caughtException = exception;
                }
                finally
                {
                    numberOfExecutions++;
                }
            }

            if (caughtException.IsNull() || retryPolicy == RetryPolicy.SwallowAllExceptions)
            {
                return;
            }

            if (ExceptionOccurred.IsNotNull())
            {
                ExceptionOccurred(this, new ExceptionEventArgs(caughtException));
            }
        }

        public TItem Retry<TItem>(Func<TItem> action, RetryPolicy retryPolicy = RetryPolicy.ThrowException, int numberOfRetries = 3)
        {
            Guard.EnsureIsNotNull("action", action);
            Guard.EnsureIsValid("retryPolicy", r => r > 0, retryPolicy);
            Guard.EnsureIsValid("numberOfRetries", i => i > 0, numberOfRetries);

            var numberOfExecutions = 0;
            Exception caughtException = null;
            var value = default(TItem);

            while (numberOfExecutions < numberOfRetries)
            {
                try
                {
                    value = action();
                }
                catch (Exception exception)
                {
                    if (numberOfExecutions != (numberOfRetries - 1))
                    {
                        continue;
                    }

                    if (retryPolicy == RetryPolicy.ThrowException)
                    {
                        throw;
                    }

                    caughtException = exception;
                }
                finally
                {
                    numberOfExecutions++;
                }
            }

            if (caughtException.IsNull() || retryPolicy == RetryPolicy.SwallowAllExceptions)
            {
                return value;
            }

            if (ExceptionOccurred.IsNotNull())
            {
                ExceptionOccurred(this, new ExceptionEventArgs(caughtException));
            }

            return value;
        }
    }
}
