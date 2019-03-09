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

            Bitmap map = GenerateHeightMap(); // <-- This is the good stuff!
            map.Save(path);

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

        private static Bitmap GenerateHeightMap()
        {
            //Instatiate a new LatticeNoiseGenerator passing the type of WaveGenerator to be applied to the lattice.
            LatticeNoiseGenerator generator = new LatticeNoiseGenerator(typeof(SineWave));

            //Create a random starting x and y point, this is to ensure that we get a random height map everytime we call GenerateHeightMap.
            Random random = new Random((int)DateTime.Now.Ticks);
            int xOffset = random.Next();
            int yOffset = random.Next();

            //Create a byte array to store all of the generated heights.
            byte[] heightMap = new byte[SIZE * SIZE];
            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    //Populate each of the points in the array one at a time, all we need is a point in coordenant space and some input variables.
                    //I have set the hypothetical size to be the same as the bitmap image we are creating, if you are generating maps without a size constraint, you can really put anything in here.
                    //The density has been set to 0.25, this means that the hypotetical size is divided into 10 lattice points each of size 10, and wave forms are attached to these points, and increased density will also increase noisyness.
                    //The frequency has been set to 0.2, this gives a nice smooth heightmap, if you want it to be noisy, try increasing to 1 or 2. If you want an even smoother map, lower it some more.
                    heightMap[x + y * SIZE] = (byte)generator.Observe(x + xOffset, y + yOffset, SIZE, 255, 0.25, 0.2);
                }
            }

            //Return a new bitmap, can't return the raw bitmap, as the converter must be disposed correctly before we can write to file.
            using (Bitmap raw = BitmapConverter.Convert(heightMap))
                return new Bitmap(raw);
        }
    }
}
