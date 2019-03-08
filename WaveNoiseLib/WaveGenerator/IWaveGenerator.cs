using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveNoiseLib.WaveGenerator
{
    /// <summary>
    /// Interface that Wave Generators are required to implement.
    /// </summary>
    /// <seealso cref="SineWave"/>
    public interface IWaveGenerator
    {
        /// <summary>
        /// Calculates a Y value between 0 and <paramref name="maxHeight"/>, at point <paramref name="distance"/> from 0 X.
        /// </summary>
        /// <param name="distance">Distance from 0 X to observe the Y value at.</param>
        /// <param name="maxHeight">Maximum Y value to return. Value is normalized to half <paramref name="maxHeight"/></param>
        /// <param name="frequency">Describes the distance between wave apexes.</param>
        /// <returns>Y value observed</returns>
        int Observe(int distance, int maxHeight, double frequency);
    }
}
