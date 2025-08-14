using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Extensions
{
    public static class IEnumerableExtention
    {
        public static SortableBindingList<T> ToSortableBindingList<T>(this IEnumerable<T> enumerable)
        {
            return new SortableBindingList<T>(enumerable.ToList());
        }

        public static string ToCommaSeparatedString<T>(this IEnumerable<T> enumerable, string separator = ", ")
        {
            return string.Join(separator, enumerable);
        }

        public static SortedList<T, V> ToSortedList<T, V>(this IEnumerable<KeyValuePair<T, V>> enumerable) where T : notnull
        {
            var list = new SortedList<T, V>();
            foreach (var item in enumerable)
            {
                list.Add(item.Key, item.Value);
            }
            return list;
        }

        public static SortedDictionary<T, V> ToSortedDictionary<T, V>(this IEnumerable<KeyValuePair<T, V>> enumerable) where T : notnull
        {
            return new SortedDictionary<T, V>(enumerable.ToDictionary(x => x.Key, x => x.Value));
        }

        public static string ToSumDollarValue(this IEnumerable<decimal?> values, int decimalCount = 2)
        {
            if (values.Where(x => !x.HasValue).Count() >= 1)
            {
                return String.Empty;
            }
            else
            {
                return values.Sum().ToDollarValue();
            }
        }
    }
}
