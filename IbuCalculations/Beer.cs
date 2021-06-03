using System.Collections.Generic;
using System.Linq;

namespace IbuCalculations
{
    public class Beer
    {
        public string Name { get; set; }
        public List<Hop> Hops { get; set; }
        public double Amount { get; set; }
        public int Ibu { get; set; }
        public double AlcoholPercentage { get; set; }
        public double MaltExtractKg { get; set; }
        public double DensityStart { get; set; }
        public double DensityEnd { get; set; }

        public Beer()
        {
            Name = null;
            Amount = 0;
            Ibu = 0;
            AlcoholPercentage = 0;
            MaltExtractKg = 0;
            DensityStart = 0;
            DensityEnd = 0;
            Hops = new List<Hop>();
        }

        public Beer(string name)
        {
            Name = name;
            Amount = 0;
            Ibu = 0;
            AlcoholPercentage = 0;
            MaltExtractKg = 0;
            DensityStart = 0;
            DensityEnd = 0;
            Hops = new List<Hop>();
        }

        public Beer(string name, double amount, int ibu, double alcohol, double malt, double densityStart, double densityEnd, List<Hop> hops)
        {
            Name = name;
            Amount = amount;
            Ibu = ibu;
            AlcoholPercentage = alcohol;
            MaltExtractKg = malt;
            DensityStart = densityStart;
            DensityEnd = densityEnd;
            Hops = hops;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Alc.: {AlcoholPercentage}, Ibu: {Ibu}";
        }

        public void MarkDuplicateHops()
        {
            var groups = Hops.GroupBy(x => x.Name)
                .Where(g => g.Count() > 1);
            foreach(var group in groups)
            {
                var itemsInGroup = group.ToArray();
                for (int i = 1; i < itemsInGroup.Count(); i++)
                {
                    itemsInGroup[i].Name += $"({i})";
                }
            }
        }
    }
}
