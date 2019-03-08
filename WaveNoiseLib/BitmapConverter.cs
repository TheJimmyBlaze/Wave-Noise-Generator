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
    public static class BitmapConverter
    {
        public static Bitmap Convert(byte[] bytes, int size)
        {
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
