using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T last, T current)> SelectWithLast<T>(this IEnumerable<T> source)
        {
            using var enumerator = source.GetEnumerator();
            enumerator.MoveNext();
            var last = enumerator.Current;
            while (enumerator.MoveNext())
            {
                yield return (last, last = enumerator.Current);
            }
        }

        public static IEnumerable<TResult> SelectWithLast<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> selector)
        {
            using var enumerator = source.GetEnumerator();
            enumerator.MoveNext();
            var last = enumerator.Current;
            do
            {
                yield return selector(last, last = enumerator.Current);
            } while (enumerator.MoveNext());
        }
    }
}