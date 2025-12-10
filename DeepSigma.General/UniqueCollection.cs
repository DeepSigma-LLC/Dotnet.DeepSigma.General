using DeepSigma.General.Extensions;
using System.Collections;

namespace DeepSigma.General;

/// <summary>
/// A collection that ensures all items are unique.
/// If a duplicate item is added, an <see cref="InvalidOperationException"/> is thrown.
/// </summary>
/// <typeparam name="T"></typeparam>
public class UniqueCollection<T> : IEnumerable<T>
{
    private HashSet<T> Collection = [];

    /// <summary>
    /// Adds an item to the collection, ensuring uniqueness.
    /// </summary>
    /// <remarks>
    /// Note: This method throws an exception (<see cref="InvalidOperationException"/>) if any item already exists in the collection.
    /// </remarks>
    /// <param name="item"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Add(T item)
    {
        if (!Collection.Add(item)) throw new InvalidOperationException($"Duplicate value: {item}");
    }

    /// <summary>
    /// Adds multiple items to the collection, ensuring uniqueness.
    /// </summary>
    /// <remarks>
    /// Note: This method throws an exception (<see cref="InvalidOperationException"/>) if any item already exists in the collection.
    /// </remarks>
    /// <param name="items"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void UnionWith(IEnumerable<T> items)
    {
        items.ForEach(item => Add(item));
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();
    

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
