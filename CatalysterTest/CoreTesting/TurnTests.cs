using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Acts;
using Catalyster.Interfaces;
using Catalyster;
using Catalyster.Components;
using Catalyster.Components.Directives;
using Catalyster.Components.Directors;
using Catalyster.Core;
using Catalyster.Messages;
using CatalysterTest.TestUtils;

namespace CatalysterTest.CoreTesting
{
    [TestClass]
    public class TurnTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.Clear();
        }

        [TestMethod]
        public void TurnTest1()
        {
            var world = World.Create();
            var order = new TurnOrder();

            ExFactory.SimpleCreature(world);
            ExFactory.SimpleCreature(world);

            var queue = order.QueryEntities(world);
            Assert.AreEqual(2, queue.Count);

            world.Dispose(); // Only ok because we made this world
        }

        [TestMethod]
        public void TurnTest2()
        {
            var world = World.Create();
            var order = new TurnOrder();

            var e1 = ExFactory.SimpleCreature(world).Reference();
            var e2 = ExFactory.SimpleCreature(world).Reference();

            e1.Entity.Add(new MonoBehavior { Directive = new RightMover { Cost = 500 } });
            e2.Entity.Add(new MonoBehavior { Directive = new RightMover { Cost = 500 } });

            var iPosX1 = e1.Entity.Get<Position>().X;
            var iPosX2 = e2.Entity.Get<Position>().X;

            order.Update(world);

            Assert.IsTrue(e1.Entity.Get<Stats>().Energy <= 0);
            Assert.IsTrue(e2.Entity.Get<Stats>().Energy <= 0);

            Assert.IsTrue(e1.Entity.Get<Position>().X > iPosX1);
            Assert.IsTrue(e2.Entity.Get<Position>().X > iPosX2);

            world.Dispose(); // Only ok because we made this world
        }

        [TestMethod]
        public void TurnTest3()
        {
            GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(10, 10);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var world = World.Create();
            var order = new TurnOrder();

            var e1 = ExFactory.SimpleCreature(world);
            ExFactory.Player(world);
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

            world.Dispose(); // Only ok because we made this world
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

            world.Dispose(); // Only ok because we made this world
        }

        [TestMethod]
        public void TurnTest5()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(30, 30);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var world = GameMaster.Instance().World;
            CommandInjectionAct.InjectedAct = null;

            var player = ExFactory.Player(world);
            var act = new DieOnPurposeAct(player.Reference());

            var hub = GameMaster.Instance().MessageLog.Hub;
            var formed = false;
            Decide? d = null;
            hub.Subscribe<ConfirmationMessage>(msg => {
                formed = true;
                Console.WriteLine(msg.Message);
                d = msg.D;
            });
            Assert.IsTrue(act.Consume().Suspended);
            Assert.IsTrue(formed);

            var torder = new TurnOrder();
            torder.SuspendedAct = act;
            if (torder.Resolve())
                torder.Update(world);
            Assert.AreEqual(act, torder.SuspendedAct);

            Assert.IsNotNull(d);
            d.Invoke(true);

            if (torder.Resolve())
                torder.Update(world);
            Assert.IsNull(torder.SuspendedAct);
        }
    }
}
