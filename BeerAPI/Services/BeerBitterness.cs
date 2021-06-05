using BeerAPI.Services.Models;
using System.Collections.Generic;

namespace BeerAPI.Services
{
    public class BeerBitterness : IBeerBitterness
    {
        public BeerBitterness()
        {

        }

        public double Bitterness(double volume, List<HopModel> hops)
        {
            var volumeGallons = volumeFiveGallons(volume);
            var bitterness = 0.0;
            foreach (var hop in hops)
            {
                var value = (hop.Utilization * hop.Weight * hop.AlphaAcid / 7.25) / volumeGallons;
                bitterness += value;
            }
            return bitterness;
        }

        private double volumeFiveGallons(double volume)
        {
            return volume * 0.264172052 / 5;
        }
    }
}
