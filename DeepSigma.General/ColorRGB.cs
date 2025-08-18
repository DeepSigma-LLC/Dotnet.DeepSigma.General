using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General
{
    /// <summary>
    /// ColorRGB is a structure that represents a color in RGB format.
    /// </summary>
    public struct ColorRGB
    {
        /// <summary>
        /// The red component of the color, ranging from 0 to 255.
        /// </summary>
        public byte Red { get; }

        /// <summary>
        /// The green component of the color, ranging from 0 to 255.
        /// </summary>
        public byte Green { get; }

        /// <summary>
        /// The blue component of the color, ranging from 0 to 255.
        /// </summary>
        public byte Blue { get; }

        /// <summary>
        /// Initializes a new instance of the ColorRGB structure.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public ColorRGB(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        /// <summary>
        /// Returns a string representation of the ColorRGB instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ColorRGB(R: {Red}, G: {Green}, B: {Blue})";
        }

        /// <summary>
        /// Creates a ColorRGB instance from a byte array.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static ColorRGB FromByteArray((byte red, byte green, byte blue) values)
        {
            return new ColorRGB(values.red, values.green, values.blue);
        }
    }
}
