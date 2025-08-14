
namespace DeepSigma.General
{
    public interface IInventoryCollection<T>
    {
        IEnumerable<T> ToIEnumerable();
        void Add(IEnumerable<T> Items);
        void Add(T item);
        void AddToFront(T item);
        void Clear();
        bool Contains(T item);
        int Count();
        T ElementAt(int Index);
        T? Peek();
        T? Pop();
        IEnumerable<Z> Select<Z>(Func<T, Z> func);
        IEnumerable<T> Where(Func<T, bool> func);
    }
}