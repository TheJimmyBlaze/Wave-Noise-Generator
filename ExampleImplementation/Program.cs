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

            LatticeNoiseGenerator generator = new LatticeNoiseGenerator(typeof(SineWave));

            Random random = new Random((int)DateTime.Now.Ticks);
            int xOffset = random.Next();
            int yOffset = random.Next();

            byte[] heightMap = new byte[SIZE * SIZE];
            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    heightMap[x + y * SIZE] = (byte)generator.Observe(x + xOffset, y + yOffset, SIZE, 255, 0.25, 0.1);
                }
            }

            Bitmap converted = BitmapConverter.Convert(heightMap, SIZE);
            Bitmap save = new Bitmap(converted);
            converted.Dispose();
            save.Save(path);

            Console.WriteLine("Created bitmap: {0}", path);
            Console.WriteLine("Open file? (Y\\N)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
                Process.Start(path);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
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
