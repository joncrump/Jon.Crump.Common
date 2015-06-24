using System;
using System.Collections.Generic;
using System.Linq;
using TightlyCurly.Com.Framework.Exceptions;
using TightlyCurly.Com.Framework.Extensions;

namespace TightlyCurly.Com.Framework
{
    public static class Guard
    {
        public static T EnsureIsNotNull<T>(string parameterName, T value)
        {
            ValidateParameterName(parameterName);

            if (value.IsNull())
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static string EnsureIsNotNullOrEmpty(string parameterName, string value)
        {
            ValidateParameterName(parameterName);

            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            if (value.Length == 0)
            {
                throw new ArgumentEmptyException(parameterName);
            }

            return value;
        }

        public static IEnumerable<T> EnsureIsNotNullOrEmpty<T>(string parameterName, IEnumerable<T> value)
        {
            ValidateParameterName(parameterName);

            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            var argumentIsNotNullOrEmpty = value as T[] ?? value.ToArray();
            if (!argumentIsNotNullOrEmpty.Any())
            {
                throw new ArgumentEmptyException(parameterName);
            }

            return argumentIsNotNullOrEmpty;
        }

        public static T EnsureIsValid<T>(string parameterName, Func<T, bool> checkPredicate, T value, string valueMessage = null)
        {
            ValidateParameterName(parameterName);

            if (!checkPredicate(value))
            {
                throw new ArgumentInvalidException(valueMessage ?? "Parameter is invalid", parameterName);
            }

            return value;
        }

        private static void ValidateParameterName(string parameterName)
        {
            if (parameterName.IsNullOrEmpty())
            {
                throw new InvalidOperationException("parameterName must be supplied.");
            }
        }
    }
}
