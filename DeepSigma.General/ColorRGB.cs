using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General
{
    public struct ColorRGB
    {
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public ColorRGB(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public override string ToString()
        {
            return $"ColorRGB(R: {Red}, G: {Green}, B: {Blue})";
        }

        public static ColorRGB FromByteArray((byte red, byte green, byte blue) values)
        {
            return new ColorRGB(values.red, values.green, values.blue);
        }
    }
}
