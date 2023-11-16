using Catalyster.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalysterTest.HelperTesting
{
    [TestClass]
    internal class WiggleHelperTest
    {
        [TestMethod]
        public void WiggleTest() // I wouldn't run this every time.
        {
            var min = 1000;
            var max = 1000;
            var rand = new Random();
            for (int i = 0; i < 10000; i++)
            {
                var x = WiggleHelper.Wiggle(1000, .1);
                if (x < min)
                    min = x;
                if (x > max)
                    max = x;
            }

            Assert.IsTrue(min < 1000);
            Assert.IsTrue(max > 1000);
        }
    }
}
