
namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for IEnumerable collections.
/// </summary>
public static class IEnumerableExtention
{
    /// <summary>
    /// Gets the most common element from a collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T? GetMostCommonElement<T>(this IEnumerable<T> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var groups = data
            .GroupBy(x => x)
            .Select(g => new { Element = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        if (groups.Count == 0)  return default;

        // Handle tie or single element
        if (groups.Count == 1 || groups[0].Count != groups[1].Count)
            return groups[0].Element;

        return default; // Tie case
    }

    /// <summary>
    /// Gets the majority element from a collection, if one exists (i.e., an element that appears more than 50% of the time).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T? GetMajorityElement<T>(this IEnumerable<T> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        List<T> list = data.ToList();

        if (list.Count == 0) return default;

        var groups = list
            .GroupBy(x => x)
            .Select(g => new { Element = g.Key, Count = g.Count() })
            .OrderByDescending(g => g.Count)
            .ToList();

        var top = groups.First();
        double percent = (double)top.Count / list.Count;

        return percent > 0.5 ? top.Element : default;
    }

    /// <summary>
    /// Converts an IEnumerable to a SortableBindingList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static SortableBindingList<T> ToSortableBindingList<T>(this IEnumerable<T> enumerable)
    {
        return new SortableBindingList<T>(enumerable.ToList());
    }

    /// <summary>
    /// Converts an IEnumerable to a comma-separated string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable"></param>
    /// <param name="seperator"></param>
    /// <returns></returns>
    public static string ToCommaSeparatedString<T>(this IEnumerable<T> enumerable, char seperator = ',')
    {
        string demlimiter = seperator.ToString();
        return string.Join(demlimiter, enumerable);
    }

    /// <summary>
    /// Converts an IEnumerable of KeyValuePairs to a SortedList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static SortedList<T, V> ToSortedList<T, V>(this IEnumerable<KeyValuePair<T, V>> enumerable) where T : notnull
    {
        var list = new SortedList<T, V>();
        foreach (var item in enumerable)
        {
            list.Add(item.Key, item.Value);
        }
        return list;
    }

    /// <summary>
    /// Converts an IEnumerable of KeyValuePairs to a SortedDictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static SortedDictionary<T, V> ToSortedDictionary<T, V>(this IEnumerable<KeyValuePair<T, V>> enumerable) where T : notnull
    {
        return new SortedDictionary<T, V>(enumerable.ToDictionary(x => x.Key, x => x.Value));
    }

    /// <summary>
    /// Calculates the sum of an IEnumerable of nullable decimals and formats it as a dollar value.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="decimalCount"></param>
    /// <returns></returns>
    public static string ToSumDollarValue(this IEnumerable<decimal?> values, int decimalCount = 2)
    {
        if (values.Where(x => !x.HasValue).Count() >= 1)
        {
            return String.Empty;
        }
        return values.Sum().ToDollarValue(decimalCount);
    }

    /// <summary>
    /// Calculates the sum of an IEnumerable of decimals and formats it as a dollar value.
    /// </summary>
    /// <param name="values"></param>
    /// <param name="decimalCount"></param>
    /// <returns></returns>
    public static string ToSumDollarValue(this IEnumerable<decimal> values, int decimalCount = 2)
    {
        return values.Sum().ToDollarValue(decimalCount);
    }
}
