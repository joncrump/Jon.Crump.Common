using System;

namespace TightlyCurly.Com.Common.Helpers
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception, RethrowPolicy policy = RethrowPolicy.Rethrow);
    }
}
