using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepSigma.General.Utilities
{
    public static class ImageUtilities
    {
        public static string ConvertImageFileToBase64String(string FullFilePath)
        {
            byte[] imageArray = File.ReadAllBytes(FullFilePath);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
            return base64ImageRepresentation;
        }

        public static byte[] ConvertImageFileToBase64ByteArray(string FullFilePath)
        {
            byte[] imageArray = File.ReadAllBytes(FullFilePath);
            return imageArray;
        }
    }
}
