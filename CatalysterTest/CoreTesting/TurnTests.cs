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
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.Clear();
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

            World.Destroy(world);
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

            Assert.IsTrue(e1.Entity.Get<Energy>().Points <= 0);
            Assert.IsTrue(e2.Entity.Get<Energy>().Points <= 0);

            Assert.IsTrue(e1.Entity.Get<Position>().X > iPosX1);
            Assert.IsTrue(e2.Entity.Get<Position>().X > iPosX2);

            World.Destroy(world);
        }

        [TestMethod]
        public void TurnTest3()
        {
            GameMaster.Instance();
            GameMaster.DungeonMap.Initialize(10, 10);
            GameMaster.DungeonMap.SetAllWalkable();
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

        [TestMethod]
        public void TurnTest5()
        {
            var gm = GameMaster.Instance();
            GameMaster.DungeonMap.Initialize(30, 30);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;
            CommandInjectionAct.InjectedAct = null;

            var player = ExFactory.Player(world);
            var act = new DieOnPurposeAct(player.Reference());

            var hub = GameMaster.MessageLog.Hub;
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
