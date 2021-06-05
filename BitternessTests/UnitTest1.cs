using Microsoft.VisualStudio.TestTools.UnitTesting;
using IbuCalculations.Services;
using System.Collections.Generic;
using IbuCalculations.Models;

namespace BitternessTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void HopUtilizationWorks()
        {
            var hop = new Hop("asd", 2, 5, 10);
            var hop2 = new Hop("asd", 2, 5, 45);
            var hop3 = new Hop("asd", 2, 5, 61);
            Assert.AreEqual(6, hop.Utilization());
            Assert.AreEqual(26.9, hop2.Utilization());
            Assert.AreEqual(31, hop3.Utilization());
        }

        [TestMethod]
        public void BitternessCalculatedCorrectlyWithOneHop()
        {
            var calc = new BeerBitternessCalculator(18.93, new List<Hop>() { new Hop("asd", 28.35f, 6, 58) });

            Assert.AreEqual(24.8, calc.Bitterness(), 0.2);
        }

        [TestMethod]
        public void BitternessCalculatedCorrectlyWithMoreHops()
        {
            var hop1 = new Hop("asd", 50, 8, 65);
            var hop2 = new Hop("asd", 20, 5, 21);
            var hop3 = new Hop("asd", 20, 4, 5);

            var list = new List<Hop>() { hop1, hop2, hop3 };

            var calc = new BeerBitternessCalculator(20, list);

            Assert.AreEqual(64.4, calc.Bitterness(), 0.2);
        }
    }
}
