namespace DeepSigma.General.Inventory;

/// <summary>
/// Implementation of custom inventory collection to achieve First-In-First-Out functionality under a consistent interface to an inventory stack.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InventoryQueueCollection<T> : InventoryCollectionAbstract<T>
{

    /// <inheritdoc cref="InventoryQueueCollection{T}"/>
    internal InventoryQueueCollection() { }

    /// <summary>
    ///  Returns the next item in the inventory collection without removing it.
    /// </summary>
    /// <returns></returns>
    public override T? Peek()
    {
        return Collection.Last();
    }

    /// <summary>
    /// Removes and returns the next item from the inventory collection.
    /// </summary>
    /// <returns></returns>
    public override T? Pop()
    {
        T? item = Collection.LastOrDefault();
        if (item is null) { return default; }
        Collection.Remove(item);
        return item;
    }

    /// <summary>
    /// Adds element to the inventory collection.
    /// </summary>
    /// <param name="item"></param>
    public override void Add(T item)
    {
        Collection.AddFirst(item);
    }

    /// <summary>
    /// Adds an item to the front of the inventory collection.
    /// </summary>
    /// <param name="item"></param>
    public override void AddToFront(T item)
    {
        Collection.AddLast(item);
    }

    /// <summary>
    /// Adds range of elements to the inventory collection.
    /// </summary>
    /// <param name="Items"></param>
    public override void Add(IEnumerable<T> Items)
    {
        foreach (var item in Items)
        {
            Add(item);
        }
    }
}
