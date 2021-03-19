using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace IbuCalculations
{
    public class BeerBitternessCalculator
    {
        private double _volume;
        public List<Hop> Hops { get; set; }
        public double Volume { get => _volume; set => setVolumeFiveGallons(value); }

        public BeerBitternessCalculator(double volume, List<Hop> hops)
        {
            setVolumeFiveGallons(volume);
            Hops = hops;
        }

        public double Bitterness()
        {
            var bitterness = 0.0;
            foreach(var hop in Hops)
            {
                var value = (hop.Utilization() * hop.Weight * hop.AlphaAcid / 7.25) / _volume;
                bitterness += value;
            }
            return bitterness;
        }

        private void setVolumeFiveGallons(double volume)
        {
            _volume = volume * 0.264172052 / 5;
        }
    }
}
