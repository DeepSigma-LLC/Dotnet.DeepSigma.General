namespace DeepSigma.General.Inventory
{
    /// <summary>
    /// Abstract class for an inventory collection that provides basic functionality for managing a collection of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class InventoryCollectionAbstract<T> : IInventoryCollection<T>
    {
        private protected LinkedList<T> Collection { get; set; } = new LinkedList<T>();

        /// <summary>
        /// Returns the collection as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> ToIEnumerable()
        {
            return Collection;
        }

        /// <summary>
        /// Clears the inventory collection.
        /// </summary>
        public void Clear()
        {
            Collection.Clear();
        }

        /// <summary>
        /// Selects elements from the collection based on a provided function.
        /// </summary>
        /// <typeparam name="Z"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public IEnumerable<Z> Select<Z>(Func<T, Z> function)
        {
            return Collection.Select(function);
        }

        /// <summary>
        /// Filters the collection based on a provided function.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public IEnumerable<T> Where(Func<T, bool> function)
        {
            return Collection.Where(function);
        }

        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Collection.Count();
        }

        /// <summary>
        /// Checks if the collection contains a specific item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return Collection.Contains(item);
        }

        /// <summary>
        /// Returns the element at the specified index in the collection.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        public T ElementAt(int Index)
        {
            return Collection.ElementAt(Index);
        }

        /// <summary>
        /// Adds a collection of items to the inventory.
        /// </summary>
        /// <param name="Items"></param>
        public abstract void Add(IEnumerable<T> Items);

        /// <summary>
        /// Adds a single item to the inventory.
        /// </summary>
        /// <param name="item"></param>
        public abstract void Add(T item);

        /// <summary>
        /// Returns the next item in the inventory collection without removing it.
        /// </summary>
        /// <returns></returns>
        public abstract T? Peek();

        /// <summary>
        /// Removes and returns the next item from the inventory collection.
        /// </summary>
        /// <returns></returns>
        public abstract T? Pop();

        /// <summary>
        /// Adds an item to the front of the inventory collection.
        /// </summary>
        /// <param name="item"></param>
        public abstract void AddToFront(T item);
    }
}
