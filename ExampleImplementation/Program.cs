using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WaveNoiseLib;
using WaveNoiseLib.WaveGenerator;

namespace ExampleImplementation
{
    class Program
    {
        private const int SIZE = 64;
        private const string EXTENSION = ".bmp";

        static void Main(string[] args)
        {
            string path = PresentInstructions();

            Regex regex = new Regex(string.Format(@"\.{0}$", EXTENSION));
            if (!regex.IsMatch(path))
                path += EXTENSION;
            
            byte[] heightMap = GenerateHeightMap(SIZE);
            WriteableBitmap bitmap = new WriteableBitmap(SIZE, SIZE, 96, 96, PixelFormats.Bgr32, null);
            Int32Rect rect = new Int32Rect(0, 0, SIZE, SIZE);
            bitmap.WritePixels(rect, heightMap, SIZE * 4, 0);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(stream);
            }

            Console.WriteLine("Created bitmap: {0}", path);
            Console.WriteLine("Open file? (Y\\N)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
                Process.Start(path);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public static byte[] GenerateHeightMap(int size)
        {
            LatticeNoiseGenerator noiseGenerator = new LatticeNoiseGenerator(typeof(SineWave));

            int pixelIndex = 0;
            byte[] pixels = new byte[size * size * 4];

            Random random = new Random((int)DateTime.Now.Ticks);
            int xSeed = random.Next();
            int ySeed = random.Next();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    byte pixelRGB = (byte)noiseGenerator.Observe(x + xSeed, y + ySeed, SIZE, 255, 0.2, 0.25);

                    for (int i = 0; i < 3; i++)
                    {
                        pixels[pixelIndex++] = pixelRGB;
                    }

                    pixels[pixelIndex++] = 255;
                }
            }

            return pixels;
        }

        private static string PresentInstructions()
        {
            Console.WriteLine("-= WaveNoiseLib Example =-");
            Console.WriteLine("This project is provided as an example of how to implement a LatticeNoiseGenerator to build a heightmap bitmap image.");
            Console.WriteLine(string.Empty);
            Console.WriteLine("Save a sample bitmap by entering a file path:");

            return Console.ReadLine();
        }
    }
}
