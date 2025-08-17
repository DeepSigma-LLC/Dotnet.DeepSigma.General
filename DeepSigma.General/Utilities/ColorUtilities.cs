using System.Drawing;

namespace DeepSigma.General.Utilities
{
    /// <summary>
    /// Utility class for color-related operations.
    /// </summary>
    public static class ColorUtilities
    {
        /// <summary>
        /// Builds a Color object from integer values representing each primary color.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static Color FromRGBIntArray((int red, int green, int blue) values)
        {
            return Color.FromArgb(values.red, values.green, values.blue);
        }
    }
}
