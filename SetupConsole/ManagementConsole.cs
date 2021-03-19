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

                var hop = new Hop("testihoppi", 0, 10.2, 0);

                da.UpsertHop(hop);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
