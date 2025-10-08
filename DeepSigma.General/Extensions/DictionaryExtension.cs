
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
    public static SortedDictionary<T, Z> ToSortedDictionary<T, Z>(this Dictionary<T, Z> dictionary) where T : notnull
    {
        return new SortedDictionary<T, Z>(dictionary);
    }
}
