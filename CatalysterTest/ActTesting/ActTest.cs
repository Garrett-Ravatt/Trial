using CatalysterTest.TestUtils;
using Catalyster.Components;
using Catalyster.Acts;
using Catalyster;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Arch.Core;
using System.Numerics;

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

            Assert.IsTrue(act.Execute());
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
        public void MeleeActTest1()
        {
            var gm = new GameMaster();
            var world = GameMaster.World;
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var c1 = ExFactory.SimpleCreature(GameMaster.World);
            var c2 = ExFactory.SimpleCreature(GameMaster.World);

            var act = new MeleeAttackAct(c1.Reference(), c2.Reference());
            Assert.IsTrue(act.Execute());
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
            Assert.IsTrue(act.Execute());
            Assert.IsTrue(item.Has<Position>());
        }
    }
}
