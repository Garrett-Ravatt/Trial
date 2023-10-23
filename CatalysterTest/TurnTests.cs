using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
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
            var order = new TurnOrder();

            ExFactory.SimpleCreature(world);
            ExFactory.SimpleCreature(world);

            var queue = order.QueryEntities(world);
            Assert.AreEqual(2, queue.Count);

            World.Destroy(world);
        }

        [TestMethod]
        public void TurnTest2()
        {
            var world = World.Create();
            var order = new TurnOrder();

            var e1 = ExFactory.SimpleCreature(world);
            var e2 = ExFactory.SimpleCreature(world);

            var iPosX1 = e1.Get<Position>().X;
            var iPosX2 = e2.Get<Position>().X;

            order.Update(world);

            Assert.IsTrue(e1.Get<Energy>().Points <= 0);
            Assert.IsTrue(e2.Get<Energy>().Points <= 0);

            Assert.IsTrue(e1.Get<Position>().X > iPosX1);
            Assert.IsTrue(e2.Get<Position>().X > iPosX2);

            World.Destroy(world);
        }

        [TestMethod]
        public void TurnTest3()
        {
            var world = World.Create();
            var order = new TurnOrder();

            var e1 = ExFactory.SimpleCreature(world);
            var player = ExFactory.Player(world);
            var e2 = ExFactory.SimpleCreature(world);

            var iPosX1 = e1.Get<Position>().X;
            var iPosX2 = e2.Get<Position>().X;

            for (var i = 0; i < 10; i++)
            {
                order.Update(world);
            }

            // Assert they won't go more than 1 tile
            Assert.IsFalse(e1.Get<Position>().X > iPosX1 + 1);
            Assert.IsFalse(e2.Get<Position>().X > iPosX2 + 1);

            World.Destroy(world);
        }

        [TestMethod]
        public void TurnTest4()
        {
            var world = World.Create();
            var order = new TurnOrder();

            ExFactory.SimpleCreature(world);
            ExFactory.SimpleCreature(world);
            var player = ExFactory.Player(world);

            Assert.AreEqual(player, order.Update(world));

            World.Destroy(world);
        }
    }
}
