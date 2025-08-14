using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Extensions
{
    public static class ByteArrayExtension
    {
        public static byte[] Combine(this byte[] Data, byte[] NewData)
        {
            return Combine2ByteArrays(Data, NewData);
        }

        private static byte[] Combine2ByteArrays(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
    }
}
