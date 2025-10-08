namespace DeepSigma.General.Inventory;

/// <summary>
/// Implementation of custom inventory collection to achieve Last-In-First-Out functionality under a consistent interface to an inventory queue.
/// </summary>
/// <typeparam name="T"></typeparam>
public class InventoryStackCollection<T> : InventoryCollectionAbstract<T>
{
    /// <summary>
    /// Implementation of custom inventory collection to acheieve Last-In-First-Out functionality under a consistent interface to a inventory queue.
    /// </summary>
    public InventoryStackCollection() { }

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
        Collection.AddFirst(item);
    }

    /// <summary>
    /// Returns the next item in the inventory collection without removing it.
    /// </summary>
    /// <returns></returns>
    public override T? Peek()
    {
        T? result = Collection.FirstOrDefault();
        if (result is null) { return default; }
        return result;
    }

    /// <summary>
    /// Removes and returns the next item from the inventory collection.
    /// </summary>
    /// <returns></returns>
    public override T? Pop()
    {
        T? item = Collection.FirstOrDefault();
        if (item is null) { return default; }
        Collection.Remove(item);
        return item;
    }
}
