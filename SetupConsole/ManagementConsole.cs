using IbuCalculations;
using System;
using System.Collections.Generic;
using IbuCalculations.Models;

namespace SetupConsole
{
    class ManagementConsole
    {
        static void Main(string[] args) 
        {
            //try
            //{
            //    var da = new DataAccess();

            //    var hop = new Hop("Hallertau Perle 2019", 1, 8, 1);
            //    var otherHop = new Hop("Hallertau 2019", 1, 6, 1);
            //    var hops = new List<Hop>() { hop, otherHop };
            //    var calc = new BeerBitternessCalculator(7, hops);
            //    var ibu = calc.Bitterness();
            //    var beer = new Beer("Vesseleiden Vuosikymmen", 20, Convert.ToInt32(59), 6.4, 4.5, 0, 0, hops);

            //    da.UpsertBeer(beer);
            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            var beer = new Beer("test", 1, 1, 1, 1, 1, 1, new List<Hop>()
            {
                new Hop("eka", 1, 1, 1),
                new Hop("toka", 1, 1, 1),
                new Hop("eka", 1, 1, 1),
                new Hop("kolmas", 1, 1, 1),
                new Hop("toka", 1, 1, 1),
                new Hop("eka", 1, 1, 1)
            });

            beer.MarkDuplicateHops();
        }
    }
}
