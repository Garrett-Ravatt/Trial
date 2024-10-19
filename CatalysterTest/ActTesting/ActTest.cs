using CatalysterTest.TestUtils;
using Catalyster.Components;
using Catalyster.Acts;
using Catalyster;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Arch.Core;
using System.Numerics;
using Catalyster.Messages;
using Microsoft.VisualBasic.FileIO;

namespace CatalysterTest.ActTesting
{
    [TestClass]
    public class ActTest
    {

        [TestMethod]
        public void ActTest1()
        {
            var act1 = new WalkAct(x: 0, y: 1);
            var act2 = new WalkAct();
        }

        [TestMethod]
        public void ActTest2()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var creature = ExFactory.SimpleCreature(GameMaster.World);
            var act = new WalkAct(GameMaster.World.Reference(creature), 0, 1);

            var Y = creature.Get<Position>().Y;
            Assert.IsNull(act.Execute());
            Assert.AreNotEqual(Y, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void ActTest3()
        {
            var gm = new GameMaster();
            var world = GameMaster.World;
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var creature = ExFactory.SimpleCreature(GameMaster.World);
            var act = new WalkAct(world.Reference(creature), 0, 1);

            var Y = creature.Get<Position>().Y;
            act.Execute();
            Assert.AreEqual(Y + 1, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void ActTest4()
        {
            var gm = new GameMaster();
            var world = GameMaster.World;
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var creature = ExFactory.SimpleCreature(GameMaster.World);
            creature.Get<Energy>().Points += 1000;
            var act = new WalkAct(creature.Reference(), 0, 1);

            var Y = creature.Get<Position>().Y;
            Assert.IsNull(act.Execute());
            Assert.AreEqual(Y + 1, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void MeleeActTest1()
        {
            var gm = new GameMaster();
            var world = GameMaster.World;
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var c1 = ExFactory.SimpleCreature(GameMaster.World);
            var c2 = ExFactory.SimpleCreature(GameMaster.World);

            var act = new MeleeAttackAct(c1.Reference(), c2.Reference());

            var formed = false;
            GameMaster.MessageLog.Hub.Subscribe<MeleeAttackMessage>(msg => formed = true);
            act.Execute();
            Assert.IsTrue(formed);
        }

        [TestMethod]
        public void ThrowActTest1()
        {
            var gm = new GameMaster();
            var world = GameMaster.World;
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var c1 = ExFactory.SimpleCreature(GameMaster.World);
            var item = world.Create(new Item { Fill = 2, Weight = 2 });
            var items = new List<EntityReference> {
                item.Reference()
            };
            c1.Add(new Inventory(items));
            var act = new ThrowAct(world.Reference(c1), 0, 1, 0);
            //TODO: verify act did resolve
            Assert.IsNull(act.Execute());
            Assert.IsTrue(item.Has<Position>());
        }
    }
}
