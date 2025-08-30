namespace DeepSigma.General.Inventory
{
    /// <summary>
    /// Implementation of custom inventory collection to achieve First-In-First-Out functionality under a consistent interface to an inventory stack.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InventoryQueueCollection<T> : InventoryCollectionAbstract<T>
    {
       
        /// <summary>
        /// Implementation of custom inventory collection to acheieve First-In-First-Out functionality under a consistent interface to a inventory stack.
        /// </summary>
        public InventoryQueueCollection(){}

        public override T? Peek()
        {
            return Collection.Last();
        }

        public override T? Pop()
        {
            T? item = Collection.LastOrDefault();
            if(item is null) { return default; }
            Collection.Remove(item);
            return item;
        }

        public override void Add(T item)
        {
            Collection.AddFirst(item);
        }

        public override void AddToFront(T item)
        {
            Collection.AddLast(item);
        }

        public override void Add(IEnumerable<T> Items)
        {
            foreach (var item in Items)
            {
                Add(item);
            }
        }
    }
}
