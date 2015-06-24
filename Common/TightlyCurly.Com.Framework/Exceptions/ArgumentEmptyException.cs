using System;

namespace TightlyCurly.Com.Framework.Exceptions
{
    public class ArgumentEmptyException : ArgumentException
    {
        public ArgumentEmptyException(string parameterName) : base(null, parameterName)
        {
        }

        public ArgumentEmptyException(string message, string parameterName) : base(message, parameterName)
        {
        }
    }
}
