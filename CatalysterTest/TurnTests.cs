using Arch.Core;
using Catalyster.Core;

namespace CatalysterTest
{
    [TestClass]
    public class TurnTests
    {
        [TestMethod]
        public void TurnTest1()
        {
            var world = World.Create();
            var map = new DungeonMap();
            var order = new TurnOrder(world, map);

            ExFactory.SimpleCreature(world);
            ExFactory.SimpleCreature(world);

            var list = order.QueryEntities();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void TurnTest2()
        {
            var world = World.Create();
            var map = new DungeonMap();
            var order = new TurnOrder(world, map);

            ExFactory.SimpleCreature(world);
            ExFactory.SimpleCreature(world);


        }
    }
}
