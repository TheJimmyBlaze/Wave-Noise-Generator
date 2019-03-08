using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveNoiseLib.WaveGenerator
{
    /// <summary>
    /// Implementation of <see cref="IWaveGenerator"/> using a simple sine wave equation.
    /// </summary>
    public class SineWave : IWaveGenerator
    {
        private readonly Random random;

        /// <summary>
        /// Instantiates an implementation of <see cref="IWaveGenerator"/> using a simple sine wave equation when observing.
        /// </summary>
        /// <param name="waveSeed">Seed used when instantiating a Random object used when observing</param>
        public SineWave(int waveSeed)
        {
            random = new Random(waveSeed);
        }

        /// <summary>
        /// Uses a simple sine wave equation to calculate a Y value between 0 and <paramref name="maxHeight"/>, at point <paramref name="distance"/> from 0 X.
        /// </summary>
        /// <param name="distance">Distance from 0 X to observe the Y value at.</param>
        /// <param name="maxHeight">Maximum Y value to return. Value is normalized to half <paramref name="maxHeight"/></param>
        /// <param name="frequency">Describes the distance between wave apexes. NOTE: this is augmented with a random value.</param>
        /// <returns>Y value observed</returns>
        public int Observe(int distance, int maxHeight, double frequency)
        {

            double zOrigin = maxHeight / 2d;
            double height = random.NextDouble() * zOrigin;
            double z = height * Math.Sin(random.NextDouble() * frequency * distance);
            double normalHeight = z + zOrigin;

            return (int)normalHeight;
        }
    }
}
