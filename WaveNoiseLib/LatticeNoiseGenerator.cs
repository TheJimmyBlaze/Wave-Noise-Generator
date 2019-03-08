using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveNoiseLib.WaveGenerator;

namespace WaveNoiseLib
{
    /// <summary>
    /// Generates noise maps using a given implementation of <see cref="IWaveGenerator"/>.
    /// </summary>
    public class LatticeNoiseGenerator
    {
        private readonly Type waveGeneratorType;

        /// <summary>
        /// Instantiates a Lattice Noise Generator to create noise maps using a given implementation of <see cref="IWaveGenerator"/>
        /// </summary>
        /// <param name="waveGeneratorType">An implementation of <see cref="IWaveGenerator"/> to use when creating lattice waves</param>
        public LatticeNoiseGenerator(Type waveGeneratorType)
        {
            this.waveGeneratorType = waveGeneratorType;
        }

        /// <summary>
        /// Observes the height of the noise map at a given <paramref name="x"/> and <paramref name="y"/> coordinant using a lattice of wave forms between the values of 0 and <paramref name="maxHeight"/>.
        /// </summary>
        /// <param name="x">X coordinant</param>
        /// <param name="y">Y coordinant</param>
        /// <param name="size">Hypothetical size of a region in which lattices are defined.</param>
        /// <param name="maxHeight">The maximum value to be returned, return value can be between 0 and <paramref name="maxHeight"/></param>
        /// <param name="density">Percentage distance of <paramref name="size"/> determining the distance between lattice points</param>
        /// <param name="frequency">Frequency to be used when osbserving the wave form</param>
        /// <returns>A Y value between 0 and <paramref name="maxHeight"/></returns>
        public int Observe(int x, int y, int size, int maxHeight, double density, double frequency)
        {
            double verticalLaticeWaveHeight = ObserveHeightAtLaticePoint(x, y, maxHeight, size * density, frequency);
            double horizontalLaticeWaveHeight = ObserveHeightAtLaticePoint(y, x, maxHeight, size * density, frequency);
            double averageHeight = (verticalLaticeWaveHeight + horizontalLaticeWaveHeight) / 2d;

            return (int)averageHeight;
        }

        /// <summary>
        /// Observes a single point of distance <paramref name="y"/> at obervation point <paramref name="x"/> between the two closest lattice points as defined by <paramref name="density"/>.
        /// </summary>
        /// <param name="x">Coordinant between the two closest lattice points</param>
        /// <param name="y">Distance along the wave forms on the lattice points</param>
        /// <param name="maxHeight">The maximum value to be returned, return value can be between 0 and <paramref name="maxHeight"/></param>
        /// <param name="density">Used when calculating how close the nearest two lattice poitns are located in coordinant space</param>
        /// <param name="frequency">Used when calcualting the wave form of each of the two lattice points</param>
        /// <returns></returns>
        private double ObserveHeightAtLaticePoint(int x, int y, int maxHeight, double density, double frequency)
        {
            double leftWaveOrigin = Math.Floor(x / density) * density;
            double rightWaveOrigin = Math.Ceiling(x / density) * density;
            double observationPoint = (x - leftWaveOrigin) / density;
            observationPoint = SmoothLatticeApproach(observationPoint);

            IWaveGenerator leftWave = Activator.CreateInstance(waveGeneratorType, (int)leftWaveOrigin) as IWaveGenerator;
            IWaveGenerator rightWave = Activator.CreateInstance(waveGeneratorType, (int)rightWaveOrigin) as IWaveGenerator;

            double leftHeight = leftWave.Observe(y, maxHeight, frequency);
            double rightHeight = rightWave.Observe(y, maxHeight, frequency);
            double waveDisparity = rightHeight - leftHeight;
            double observableWaveHeight = leftHeight + waveDisparity * observationPoint;

            return observableWaveHeight;
        }

        /// <summary>
        /// Smooths the approach towards a latice point to prevent height calculations being observed at points 0 or 1.
        /// e.g. 0.9 => 0.98, or 0.1 => 0.02
        /// </summary>
        /// <param name="latice">The lattice origin point to be smoothed</param>
        /// <returns>The result of smoothing on the provided lattice origin point</returns>
        private double SmoothLatticeApproach(double latice)
        {
            return (1 - Math.Abs(latice - 0.5d)) * (2 - double.Epsilon) * (latice - 0.5d) + 0.5d;
        }
    }
}
