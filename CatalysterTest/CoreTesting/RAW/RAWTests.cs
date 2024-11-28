using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Core;
using Catalyster.RAW;
using CatalysterTest.TestUtils;

namespace CatalysterTest.CoreTesting.RAW
{
    // Tests RAW definitions without GameMaster

    [TestClass]
    public class RAWTests
    {
        private EntityStats stats;

        [TestInitialize]
        public void Initialize()
        {
            stats = new EntityStats();
        }

        [TestCleanup]
        public void Cleanup()
        {
            stats.World.Dispose();
        }

        [TestMethod]
        public void TestRaw1()
        {
            var c = ExFactory.SimpleCreature(stats.World);
            var def = new EntityDefinition("creature", "There it is, a widdle fella", c.Reference());
            var rid = c.Get<Token>().RID;
            Assert.IsFalse(stats.Has(rid));
            stats.Define(def);
            Assert.IsTrue(stats.Has(rid));
            Assert.AreEqual(def, stats.Get(rid));
        }

        [TestMethod]
        public void TestRaw2()
        {
            var c = ExFactory.BlackPowder(stats.World);
            var def = new EntityDefinition("Black Powder", "Traditional. Versatile.", c.Reference());
            
            var rid = c.Get<Token>().RID;
            stats.Define(def);

            var world = World.Create();
            var e = stats.CreateIn(rid, world);
            Assert.AreEqual(c.Get<Token>(), e.Get<Token>());

            world.Dispose();
        }

        [TestMethod]
        public void TestRaw3()
        {
            var world = World.Create();

            var e = RAWFactory.BlackPowder(stats, world);
            Assert.AreEqual(e.Get<Token>(), e.Get<Token>());

            world.Dispose();
        }
    }
}
