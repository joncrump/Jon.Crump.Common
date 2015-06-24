namespace TightlyCurly.Com.Common
{
    public enum RetryPolicy
    {
        ThrowException = 1,
        SwallowExceptionAndSendToEvent,
        SwallowAllExceptions,
    }
}
