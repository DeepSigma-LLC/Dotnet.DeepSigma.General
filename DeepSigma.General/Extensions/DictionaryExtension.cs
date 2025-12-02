
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DeepSigma.General.Extensions;

/// <summary>
/// Extension methods for Dictionary operations.
/// </summary>
public static class DictionaryExtension
{
    /// <summary>
    /// Converts a Dictionary to a SortedDictionary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="dictionary"></param>
    /// <returns></returns>
    public static SortedDictionary<T, Z> ToSortedDictionary<T, Z>(this IDictionary<T, Z> dictionary) where T : notnull
    {
        return new SortedDictionary<T, Z>(dictionary);
    }

    /// <summary>
    /// Tries to get a value from the dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static (bool found, TValue? value) TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        ArgumentNullException.ThrowIfNull(dict);
        return dict.TryGetValue(key, out var value) ? (true, value) : (false, default);
    }

    /// <summary>
    /// Increments the integer value associated with the specified key in the dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="amount"></param>
    public static void Increment<TKey>(this IDictionary<TKey, int> dict, TKey key, int amount = 1)
    {
        if (dict.ContainsKey(key))
        {
            dict[key] += amount; 
            return;
        }         
        dict[key] = amount;
    }

    /// <summary>
    /// Converts the dictionary to a readable string format.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <returns></returns>
    public static string ToReadableString<TKey, TValue>(this IDictionary<TKey, TValue> dict)
    {
        return "{" + string.Join(", ", dict.Select(kvp => $"{kvp.Key}: {kvp.Value}")) + "}";
    }

    /// <summary>
    /// Gets the value associated with the specified key, or adds the key with the provided value if it does not exist.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static TValue? GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue? value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        if(exists) return val;
        val = value;
        return val;
    }

    /// <summary>
    /// Tries to update the value associated with the specified key in the dictionary. 
    /// Returns true if the update was successful, false if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
        if (Unsafe.IsNullRef(ref val)) return false;
        val = value;
        return true;
    }
}
