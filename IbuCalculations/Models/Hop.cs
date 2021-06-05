using System;
using System.Collections.Generic;
using System.Text;

namespace IbuCalculations.Models
{
    public class Hop
    {
        private double _weight;
        public double AlphaAcid { get; set; }
        public string Name { get; set; }
        public double Weight { get => getWeightOunces(); set => _weight = value; }

        public double WeightGrams { get => _weight; }
        public int BoilingTime { get; set; }

        public Hop()
        {
            _weight = 0;
            AlphaAcid = 0;
            BoilingTime = 0;
            Name = null;
        }

        public Hop(string name, double weight, double alpha, int boilingTime)
        {
            _weight = weight;
            AlphaAcid = alpha;
            BoilingTime = boilingTime;
            Name = name;
        }

        public double Utilization()
        {
            var level =(BoilingTime - 1) / 5;
            switch (level)
            {
                case 0: return 5;
                case 1: return 6;
                case 2: return 8;
                case 3: return 10.1;
                case 4: return 12.1;
                case 5: return 15.3;
                case 6: return 18.8;
                case 7: return 22.8;
                case 8: return 26.9;
                case 9: return 28.1;
                case 10: return 30;
                case 11: return 30;
                default: return 31;
            }
        }
        private double getWeightOunces()
        {
            return _weight * 0.0352739619;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Alpha acid: {AlphaAcid}, Weight: {_weight}, Boiling time: {BoilingTime}";
        }
    }
}
