namespace DeepSigma.General.Inventory
{
    /// <summary>
    /// Defines a generic inventory collection interface with common collection operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInventoryCollection<T>
    {
        /// <summary>
        /// Returns the collection as an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> ToIEnumerable();
        /// <summary>
        /// Adds a range of elements to the inventory collection.
        /// </summary>
        /// <param name="Items"></param>
        void Add(IEnumerable<T> Items);

        /// <summary>
        /// Adds an element to the inventory collection.
        /// </summary>
        /// <param name="item"></param>
        void Add(T item);
        /// <summary>
        /// Adds an item to the front of the inventory collection.
        /// </summary>
        /// <param name="item"></param>
        void AddToFront(T item);
        /// <summary>
        /// Clears all elements from the inventory collection.
        /// </summary>
        void Clear();
        /// <summary>
        /// Determines if the collection contains a specific item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(T item);
        /// <summary>
        /// Returns the number of elements in the collection.
        /// </summary>
        /// <returns></returns>
        int Count();
        /// <summary>
        /// Returns the element at the specified index.
        /// </summary>
        /// <param name="Index"></param>
        /// <returns></returns>
        T ElementAt(int Index);
        /// <summary>
        /// Returns the next item in the inventory collection without removing it.
        /// </summary>
        /// <returns></returns>
        T? Peek();
        /// <summary>
        /// Removes and returns the next item from the inventory collection.
        /// </summary>
        /// <returns></returns>
        T? Pop();

        /// <summary>
        /// Selects a series of elements.
        /// </summary>
        /// <typeparam name="Z"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        IEnumerable<Z> Select<Z>(Func<T, Z> function);

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        IEnumerable<T> Where(Func<T, bool> function);
    }
}