
namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for IEnumerable collections.
/// </summary>
public static class IEnumerableExtension
{
    /// <summary>
    /// Determines if an IEnumerable is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source) => source == null || !source.Any();

    /// <summary>
    /// Returns an empty IEnumerable if the source is null; otherwise, returns the source itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? source) => source ?? [];

    /// <summary>
    /// Performs the specified action on each element of the IEnumerable and returns the original IEnumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item); 
        }
        return source;
    }

    /// <summary>
    /// Filters out null values from an IEnumerable of nullable reference types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class => source.Where(x => x != null)!;

    /// <summary>
    /// Filters out elements that match the given predicate.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// // var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// // var evenNumbers = numbers.WhereNot(n => n % 2 == 0); // Result: { 1, 3, 5 }
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereNot<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source.Where(x => !predicate(x));


    /// <summary>
    /// Returns distinct elements from a sequence by using a specified key selector to compare values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) => source.GroupBy(keySelector).Select(g => g.First());

    /// <summary>
    /// Determines whether no elements of a sequence satisfy a condition or if the sequence is empty.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var numbers = new List&lt;int&gt; { 1, 2, 3 };
    /// bool noneGreaterThanThree = numbers.None(n => n > 3); // Result: true
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool None<T>(this IEnumerable<T> source, Func<T, bool>? predicate = null) => predicate == null ? !source.Any() : !source.Any(predicate);

    /// <summary>
    /// Gets the most common element from a collection.
    /// If no element is most common, such as in the case of a tie, returns default.
    /// Note: Elements must be comparable for grouping. This means that for custom types, Equals and GetHashCode should be properly overridden.
    /// Record types in C# automatically provide these implementations. Structs also provide value-based equality by default.
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
    /// Gets the least common element from a collection.
    /// If no element is least common, such as in the case of a tie, returns default.
    /// Note: Elements must be comparable for grouping. This means that for custom types, Equals and GetHashCode should be properly overridden.
    /// Record types in C# automatically provide these implementations. Structs also provide value-based equality by default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T? GetLeastCommonElement<T>(this IEnumerable<T> data)
    {
        ArgumentNullException.ThrowIfNull(data);

        var groups = data
            .GroupBy(x => x)
            .Select(g => new { Element = g.Key, Count = g.Count() })
            .OrderBy(g => g.Count) // Order ascending for least common
            .ToList();

        if (groups.Count == 0) return default;

        // Handle tie or single element
        if (groups.Count == 1 || groups[0].Count != groups[1].Count)
            return groups[0].Element;

        return default; // Tie case
    }

    /// <summary>
    /// Gets the majority element from a collection, if one exists (i.e., an element that appears more than 50% of the time).
    /// If no majority element exists, returns default.
    /// Note: Elements must be comparable for grouping. This means that for custom types, Equals and GetHashCode should be properly overridden.
    /// Record types in C# automatically provide these implementations. Structs also provide value-based equality by default.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="majority_threshold">Optional parameter that defaults to 0.5 (aka 50%) which indicates that a majority is determined by an element that occurs > 50% of the time.</param>
    /// <returns></returns>
    public static T? GetMajorityElement<T>(this IEnumerable<T> data, double majority_threshold = 0.5)
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

        return percent > majority_threshold ? top.Element : default;
    }

    private static readonly Random _random = new();

    /// <summary>
    /// Chooses a random element from an IEnumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static T ChooseRandomElement<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        IList<T> list = source as IList<T> ?? source.ToList();
        if (list.Count == 0) throw new InvalidOperationException("Sequence contains no elements");

        return list[_random.Next(list.Count)];
    }

    /// <summary>
    /// Shuffles the elements of an IEnumerable into a random order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var rng = new Random();
        return source.OrderBy(_ => rng.Next());
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
    /// Joins the elements of an IEnumerable into a single string with the specified separator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string JoinAsString<T>(this IEnumerable<T> source, string separator = ", ") => string.Join(separator, source);


    /// <summary>
    /// Projects each element of an IEnumerable into a new form.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var numbers = new List&lt;int&gt; { 1, 2, 3 };
    /// var squares = numbers.Map(n => n * n); // Result: { 1, 4, 9 }
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> source, Func<T, TResult> selector) => source.Select(selector);

    /// <summary>
    /// Filters an IEnumerable based on a predicate.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// var evenNumbers = numbers.Filter(n => n % 2 == 0); // Result: { 2, 4 }
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>  source.Where(predicate);

    /// <summary>
    /// Divides an IEnumerable into chunks of a specified size.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// var chunks = numbers.ChunkBy(2); // Result: { {1, 2}, {3, 4}, {5} }
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int size)
    {
        if (size <= 0) throw new ArgumentException("Size must be greater than zero.", nameof(size));

        List<T> chunk = new(size);
        foreach (T item in source)
        {
            chunk.Add(item);
            if (chunk.Count == size)
            {
                yield return chunk.ToList();
                chunk.Clear();
            }
        }

        if (chunk.Count != 0)
            yield return chunk.ToList();
    }

    /// <summary>
    /// Projects each element of an IEnumerable into a tuple containing the element and its index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source) => source.Select((item, index) => (item, index));

    /// <summary>
    /// Partitions an IEnumerable into two collections based on a predicate.
    /// The first collection contains elements that match the predicate, and the second contains elements that do not.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var numbers = new List&lt;int&gt; { 1, 2, 3, 4, 5 };
    /// var (evenNumbers, oddNumbers) = numbers.Partition(n => n % 2 == 0);
    /// // evenNumbers: { 2, 4 }
    /// // oddNumbers: { 1, 3, 5 }
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static (IEnumerable<T> matches, IEnumerable<T> nonMatches) Partition<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        List<T> matches = [];
        List<T> nonMatches = [];

        foreach (var item in source)
        {
            if (predicate(item))
            {
                matches.Add(item);
            }
            else
            {
                nonMatches.Add(item);
            }
        }
        return (matches, nonMatches);
    }

    /// <summary>
    /// Groups the elements of an IEnumerable according to a specified key selector function.
    /// Handles null source by returning an empty collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static IEnumerable<IGrouping<TKey, T>> GroupBySafe<T, TKey>(
    this IEnumerable<T> source, Func<T, TKey> keySelector)
    {
        return source?.GroupBy(keySelector) ?? [];
    }


    /// <summary>
    /// Attempts to perform the specified action on each element of the IEnumerable, swallowing any exceptions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> TryAction<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            try { action(item); }
            catch { /* swallow or log */ }
        }
        return source;
    }

    /// <summary>
    /// Flattens a nested IEnumerable into a single IEnumerable.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source)
    {
        foreach (var sublist in source)
        {
            foreach (var item in sublist)
            {
                yield return item;
            }
        }
    }

    /// <summary>
    /// Returns the top N elements from an IEnumerable based on a specified key selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Top<T, TKey>(this IEnumerable<T> source, int count, Func<T, TKey> predicate)
    {
        return source.OrderByDescending(predicate).Take(count);
    }

    /// <summary>
    /// Returns the bottom N elements from an IEnumerable based on a specified key selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="source"></param>
    /// <param name="count"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Bottom<T, TKey>(this IEnumerable<T> source, int count, Func<T, TKey> predicate)
    {
        return source.OrderBy(predicate).Take(count);
    }

    /// <summary>
    /// Calculates the weighted average of a collection.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var items = new List&lt;Item&gt; { new Item { Value = 10, Weight = 2 }, new Item { Value = 20, Weight = 3 } };
    /// var weightedAvg = items.WeightedAverage(i => i.Value, i => i.Weight); // Result: 16
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="valueSelector"></param>
    /// <param name="weightSelector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static decimal WeightedAverage<T>(this IEnumerable<T> source, Func<T, decimal> valueSelector, Func<T, decimal> weightSelector)
    {
        ArgumentNullException.ThrowIfNull(source);

        decimal weightedSum = 0;
        decimal totalWeight = 0;

        foreach (T item in source)
        {
            decimal value = valueSelector(item);
            decimal weight = weightSelector(item);
            weightedSum += value * weight;
            totalWeight += weight;
        }

        return totalWeight == 0 ? 0 : weightedSum / totalWeight;
    }

    /// <summary>
    /// Calculates the running total of a sequence of decimal values.
    /// </summary>
    /// <remarks>
    /// <code>
    /// // Example usage:
    /// var numbers = new List&lt;decimal&gt; { 1.0m, 2.0m, 3.0m };
    /// var runningTotals = numbers.RunningTotal(n => n); // Result: { 1.0m, 3.0m, 6.0m }
    /// </code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<decimal> RunningTotal<T>(this IEnumerable<T> source, Func<T, decimal> selector)
    {
        ArgumentNullException.ThrowIfNull(source);

        decimal sum = 0;
        foreach (T item in source)
        {
            sum += selector(item);
            yield return sum;
        }
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
    /// Converts an IEnumerable of tuples to a SortedDictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static SortedDictionary<T, V> ToSortedDictionary<T, V>(this IEnumerable<(T Key, V Value)> enumerable) where T : notnull
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
