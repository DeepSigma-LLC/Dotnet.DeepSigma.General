using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.DistributedData.Interface
{
    /// <summary>
    /// Interface for a Bloom filter that supports item removal.
    /// </summary>
    public interface IBloomFilterRemovable: IBloomFilter
    {
        void Remove(string item);
    }
}
