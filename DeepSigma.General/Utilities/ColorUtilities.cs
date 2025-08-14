using System.Drawing;

namespace DeepSigma.General.Utilities
{
    public static class ColorUtilities
    {
        public static Color FromRGBIntArray(int[] RGBColorArray)
        {
            return Color.FromArgb(RGBColorArray[0], RGBColorArray[1], RGBColorArray[2]);
        }
    }
}
