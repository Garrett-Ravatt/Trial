using Catalyster.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Catalyster.Hunks;

namespace CatalysterTest.ComponentTesting
{
    [TestClass]
    public class ExplosiveTest
    {
        [TestMethod]
        public void ExplosiveTest1()     
        {
            //
            var hunk1 = new IntHunk(new int[] { 0, 1, 2 });
            var hunk2 = new IntHunk(new int[] { 0, 1, 2 });
            Assert.AreEqual(hunk1, hunk2);
        }

        [TestMethod]
        public void ExplosiveTest2()
        {
            //
        }
    }
}
