namespace DeepSigma.General.DistributedData.Interface
{
    /// <summary>
    /// Bloom filter interface.
    /// </summary>
    public interface IBloomFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Add(string item);
        bool MightContain(string item);
        (int number_of_slots, int number_of_hashes, long number_of_adds, long number_of_removes, int non_zero_slots) Parameters();
        public double EstimatedFill();
    }
}