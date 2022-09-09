namespace IbuCalculations.Models
{
    public class Hop
    {
        private double _weight;
        public double AlphaAcid { get; set; }
        public string Name { get; set; }
        public double Weight { get => GetWeightOunces(); set => _weight = value; }

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
            // Handle 0 as dry hop -> no bitterness yield
            if (BoilingTime == 0)
            {
                return 0;
            }

            var level =(BoilingTime - 1) / 5;
            return level switch
            {
                0 => 5,
                1 => 6,
                2 => 8,
                3 => 10.1,
                4 => 12.1,
                5 => 15.3,
                6 => 18.8,
                7 => 22.8,
                8 => 26.9,
                9 => 28.1,
                10 => 30,
                11 => 30,
                _ => 31,
            };
        }
        private double GetWeightOunces()
        {
            return _weight * 0.0352739619;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Alpha acid: {AlphaAcid}, Weight: {_weight}, Boiling time: {BoilingTime}";
        }
    }
}
