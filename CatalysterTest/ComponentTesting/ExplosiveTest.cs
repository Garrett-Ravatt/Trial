using Catalyster.Components;
using Catalyster.Hunks;
using Catalyster.Helpers;

namespace CatalysterTest.ComponentTesting
{
    [TestClass]
    public class ExplosiveTest
    {
        [TestMethod]
        public void ExplosiveTest1()
        {
            var explosive = new Explosive
            {
                Resistance = new IntHunk(new int[] { 0, 0 }),
                Potential = new IntHunk(new int[] { 1, 1 }),
            };

            Assert.IsNotNull(explosive);
        }

        [TestMethod]
        public void ExplosiveTest2()
        {
            // test a detonation check

            var explosive = new Explosive
            {
                Resistance = new IntHunk(new int[] { 0, 0 }),
                Potential = new IntHunk(new int[] { 1, 1 }),
            };

            Assert.IsTrue(DetonationHelper.Detonates(DetonationHelper.Fuse(), explosive));
        }

        [TestMethod]
        public void ExplosiveTest3()
        {
            var explosives = new List<Explosive>
            {
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 0 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 2, 2 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 2, 3 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 1 }),
                    Potential = new IntHunk(new int[] { 0, 0 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 1 }),
                    Potential = new IntHunk(new int[] { 0, 2 })
                },
            };

            CollectionAssert.AreEquivalent(new bool[]{ true, true, false, true, true }, DetonationHelper.Detonate(explosives));
        }

        [TestMethod]
        public void ExplosiveTest4()
        {
            var explosives = new List<Explosive>
            {
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 0 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 2, 2 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 2, 3 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 1 }),
                    Potential = new IntHunk(new int[] { 0, 0 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 1 }),
                    Potential = new IntHunk(new int[] { 0, 2 })
                },
            };

            var detonated = DetonationHelper.Detonated(explosives);
            Assert.AreEqual(4, detonated.Count);
        }

        [TestMethod]
        public void ExplosiveTest5()
        {
            var explosives = new List<Explosive>
            {
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 0 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 2, 2 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 2, 3 }),
                    Potential = new IntHunk(new int[] { 1, 1 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 1 }),
                    Potential = new IntHunk(new int[] { 0, 0 })
                },
                new Explosive
                {
                    Resistance = new IntHunk(new int[] { 0, 1 }),
                    Potential = new IntHunk(new int[] { 0, 2 })
                },
            };

            var formula = DetonationHelper.DamageDice(explosives);
            Console.WriteLine(formula.ToString());
            
            Assert.IsTrue(formula.Roll().Value > 0);
            Assert.IsNotNull(formula);
        }
    }
}
