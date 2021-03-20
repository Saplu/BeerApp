using System;
using System.Collections.Generic;
using System.Text;
using IbuCalculations;

namespace SetupConsole
{
    class ManagementConsole
    {
        static void Main(string[] args) 
        {
            try
            {
                var da = new DataAccess();

                var hop = new Hop("Hallertau Perle", 42, 8, 60);
                var otherHop = new Hop("Hallertau Spalt Select", 56, 6, 15);
                var hops = new List<Hop>() { hop, otherHop };
                var calc = new BeerBitternessCalculator(20, hops);
                var ibu = calc.Bitterness();
                var beer = new Beer("Vesseleiden Vuosikymmen", 20, Convert.ToInt32(ibu), 6.4, 4.5, 0, 0, hops);

                da.UpsertBeer(beer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
