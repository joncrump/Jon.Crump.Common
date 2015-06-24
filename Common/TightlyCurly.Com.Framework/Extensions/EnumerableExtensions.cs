using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace TightlyCurly.Com.Framework.Extensions
{
    public static class EnumerableExtensions
    {
        public static int SafeCount<TValue>(this IEnumerable<TValue> values)
        {
            if (values.IsNull())
            {
                return 0;
            }

            return values.Count();
        }

        public static TValue SafeGet<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values, TKey key,
            Func<TValue> defaultValue = null)
        {
            if (values.IsNullOrEmpty())
            {
                return defaultValue.IsNull() ? default(TValue) : defaultValue();
            }

            var dictionary = values.ToDictionary(x => x.Key, x => x.Value);

            return !dictionary.ContainsKey(key)
                ? defaultValue.IsNull() ? default(TValue) : defaultValue()
                : dictionary[key];
        }

        public static TReturn SafeGetAs<TKey, TValue, TReturn>(this IEnumerable<KeyValuePair<TKey, TValue>> values, TKey key,
            Func<TReturn> defaultValue = null, bool ignoreCase = true)
        {
            if (values.IsNullOrEmpty())
            {
                return defaultValue.IsNull()
                    ? default(TReturn) : defaultValue();
            }

            var dictionary = values.ToDictionary(x => x.Key, x => x.Value);

            return !dictionary.ContainsKey(key)
                ? defaultValue.IsNull() ? default(TReturn) : defaultValue()
                : (TReturn)Convert.ChangeType(dictionary[key], typeof(TReturn));
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> values)
        {
            return !IsNullOrEmpty(values);
        }

        public static void SafeForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            foreach (var value in values)
            {
                action(value);
            }
        }

        public static IEnumerable<TItem> MergeWith<TItem>(this IEnumerable<TItem> list1, IEnumerable<TItem> list2)
        {
            IEnumerable<TItem> value = new List<TItem>();

            if (list1.IsNullOrEmpty() && list2.IsNotNullOrEmpty())
            {
                value = list2;
            }
            else if (list1.IsNotNullOrEmpty() && list2.IsNullOrEmpty())
            {
                value = list1;
            }
            else if (list1.IsNotNullOrEmpty() && list2.IsNotNullOrEmpty())
            {
                value = list1.Concat(list2);
            }

            return value;
        }

        public static bool IsNullOrEmpty(this NameValueCollection nameValues)
        {
            if (nameValues.IsNull())
            {
                return true;
            }

            return nameValues.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> values)
        {
            return values == null || !values.Any();
        }

        public static string Implode<T>(this IEnumerable<T> values, string delimiter)
        {
            return values.IsNullOrEmpty() 
                ? String.Empty 
                : String.Join(delimiter, values.Select(v => v.ToString()).ToArray());
        }

        public static void Each<T>(this IEnumerable<T> values, Action<T> action)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            foreach (var value in values)
            {
                action(value);
            }
        }

        public static T[] ToSafeArray<T>(this IEnumerable<T> values)
        {
            return values.IsNull() ? new T[5] : values.ToArray();
        }

        public static IList<T> ToSafeList<T>(this IEnumerable<T> values)
        {
            return values == null ? new List<T>() : values.ToList();
        }

        public static IEnumerable<T> SafeWhere<T>(this IEnumerable<T> values, Func<T, bool> predicate)
        {
            if (values.IsNull() || predicate.IsNull())
            {
                return new List<T>();
            }

            return values.Where(predicate);
        }

        public static IEnumerable<TResult> SafeSelect<T, TResult>(this IEnumerable<T> values, Func<T, TResult> selector)
        {
            if (values.IsNull() || selector.IsNull())
            {
                return new List<TResult>();
            }

            return values.Select(selector);
        }

        public static IEnumerable<T> DoIfDefault<T>(this IEnumerable<T> value, Func<IEnumerable<T>> doDelegate)
        {
            if (value.IsNullOrEmpty())
            {
                return doDelegate();
            }

            return value;
        }

        public static bool EnumerableEquals<T>(this IEnumerable<T> theseValues, IEnumerable<T> otherValues)
        {
            return EnumerableEquals<T, object>(theseValues, otherValues);
        }

        public static bool EnumerableEquals<T, TKey>(this IEnumerable<T> theseValues, IEnumerable<T> otherValues,
             Func<T, TKey> sortDelegate = null, Func<T, T, bool> comparisonDelegate = null)
        {
            if (theseValues.IsNullOrEmpty() && otherValues.IsNullOrEmpty())
            {
                return true;
            }

            if (theseValues.IsNullOrEmpty() && !otherValues.IsNullOrEmpty())
            {
                return false;
            }

            if (!theseValues.IsNullOrEmpty() && otherValues.IsNullOrEmpty())
            {
                return false;
            }

            if (theseValues.Count() != otherValues.Count())
            {
                return false;
            }

            T[] tempThese;
            T[] tempOthers;

            if (sortDelegate == null)
            {
                tempThese = theseValues.ToArray();
                tempOthers = otherValues.ToArray();
            }
            else
            {
                tempThese = theseValues.OrderBy(sortDelegate).ToArray();
                tempOthers = otherValues.OrderBy(sortDelegate).ToArray();
            }

            if (comparisonDelegate == null)
            {
                comparisonDelegate = (e, a) => e.Equals(a);
            }

            return !tempThese.Where((t, index) => !comparisonDelegate(t, tempOthers[index])).Any();
        }
    }
}
