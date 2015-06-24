using System;

namespace TightlyCurly.Com.Common.Helpers
{
    public delegate void ExceptionOccurredHandler(object sender, ExceptionEventArgs e);

    public interface IRetryHelper
    {
        event ExceptionOccurredHandler ExceptionOccurred;
        void Retry(Action action, RetryPolicy retryPolicy = RetryPolicy.ThrowException, int numberOfRetries = 3);
        TItem Retry<TItem>(Func<TItem> action, RetryPolicy retryPolicy = RetryPolicy.ThrowException,
            int numberOfRetries = 3);
    }
}