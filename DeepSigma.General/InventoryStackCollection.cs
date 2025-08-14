using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General
{
    public class InventoryStackCollection<T> : InventoryCollectionAbstract<T>
    {
        /// <summary>
        /// Implementation of custom inventory collection to acheieve Last-In-First-Out functionality under a consistent interface to a inventory queue.
        /// </summary>
        public InventoryStackCollection(){}

        public override void Add(IEnumerable<T> Items)
        {
            foreach (var item in Items)
            {
                Add(item);
            }
        }

        public override void Add(T item)
        {
            Collection.AddFirst(item);
        }

        public override void AddToFront(T item)
        {
            Collection.AddFirst(item);
        }

        public override T? Peek()
        {
            T? result = Collection.FirstOrDefault();
            if(result is null) { return default; }
            return result;
        }

        public override T? Pop()
        {
            T? item = Collection.FirstOrDefault();
            if(item is null) { return default; }
            Collection.Remove(item);
            return item;
        }
    }
}
