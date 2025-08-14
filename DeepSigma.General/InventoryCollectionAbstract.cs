

namespace DeepSigma.General
{
    public abstract class InventoryCollectionAbstract<T> : IInventoryCollection<T>
    {
        private protected LinkedList<T> Collection { get; set; } = new LinkedList<T>();

        public IEnumerable<T> ToIEnumerable()
        {
            return Collection;
        }

        public void Clear()
        {
            Collection.Clear();
        }

        public IEnumerable<Z> Select<Z>(Func<T, Z> func)
        {
            return Collection.Select(func);
        }

        public IEnumerable<T> Where(Func<T, bool> func)
        {
            return Collection.Where(func);
        }

        public int Count()
        {
            return Collection.Count();
        }

        public bool Contains(T item)
        {
            return Collection.Contains(item);
        }

        public T ElementAt(int Index)
        {
            return Collection.ElementAt(Index);
        }

        public abstract void Add(IEnumerable<T> Items);

        public abstract void Add(T item);

        public abstract T? Peek();

        public abstract T? Pop();

        public abstract void AddToFront(T item);
    }
}
