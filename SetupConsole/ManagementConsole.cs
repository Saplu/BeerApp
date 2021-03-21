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

                var hop = new Hop("testi", 12, 6, 60);
                var otherHop = new Hop("ihanuus", 16, 8.5, 15);
                var hops = new List<Hop>() { hop, otherHop };
                var calc = new BeerBitternessCalculator(20, hops);
                var ibu = calc.Bitterness();
                var beer = new Beer("olut", 10, Convert.ToInt32(ibu), 5, 1.5, 0, 0, hops);

                da.UpsertBeer(beer);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
