using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Extensions
{
    public static class DictionaryExtension
    {
        public static SortedDictionary<T, Z> ToSortedDictionary<T, Z>(this Dictionary<T, Z> dictionary) where T : notnull
        {
            return new SortedDictionary<T, Z>(dictionary);
        }
    }
}
