using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WaveNoiseLib
{
    /// <summary>
    /// Converts byte arrays created by <see cref="LatticeNoiseGenerator"/> into bitmap images for increased versatility.
    /// </summary>
    public static class BitmapConverter
    {
        /// <summary>
        /// Takes a byte array containing heights in coordinant space and returns them output as a bitmap image.
        /// NOTE: this method will only work for byte arrays where the width and height are equal lengths.
        /// </summary>
        /// <param name="bytes">Array of bytes to be converted into a bitmap, this is normally the output form <see cref="LatticeNoiseGenerator"/></param>
        /// <returns>A bitmap image</returns>
        public static Bitmap Convert(byte[] bytes)
        {
            int size = (int)Math.Sqrt(bytes.Length);

            int pixelIndex = 0;
            byte[] pixels = new byte[size * size * 4];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    byte pixelRGB = bytes[x + y * size];

                    for (int i = 0; i < 3; i++)
                    {
                        pixels[pixelIndex++] = pixelRGB;
                    }

                    pixels[pixelIndex++] = 255;
                }
            }

            WriteableBitmap bitmap = new WriteableBitmap(size, size, 96, 96, PixelFormats.Bgr32, null);
            Int32Rect rect = new Int32Rect(0, 0, size, size);
            bitmap.WritePixels(rect, pixels, size * 4, 0);

            using (MemoryStream stream = new MemoryStream())
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)bitmap));
                encoder.Save(stream);
                return new Bitmap(stream);
            }
        }
    }
}
