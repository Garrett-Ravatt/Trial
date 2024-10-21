using CatalysterTest.TestUtils;
using Catalyster.Components;
using Catalyster.Acts;
using Catalyster.Interfaces;
using Catalyster;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Arch.Core;
using Catalyster.Messages;

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
            Assert.AreEqual(act, act.Execute());
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
            Assert.IsTrue(act.Execute().Resolved);
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
            Assert.IsTrue(act.Execute().Resolved);
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
            Assert.IsTrue(act.Execute().Resolved);
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
            
            Assert.IsTrue(act.Execute().Resolved);
            Assert.IsTrue(item.Has<Position>());
        }

        [TestMethod]
        public void ProbeActTest1()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            GameMaster.DungeonMap.SetCellProperties(0, 1, false, false);

            var creature = ExFactory.Player(GameMaster.World);
            var act = new ProbeAct(GameMaster.World.Reference(creature), 0, 1);

            var formed = false;
            GameMaster.MessageLog.Hub.Subscribe<WallBumpMessage>(msg => formed = true);
            Assert.IsTrue(act.Execute().Resolved);
            Assert.IsTrue(formed);
        }

        [TestMethod]
        public void AlternateActTest()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            GameMaster.DungeonMap.SetCellProperties(0, 1, false, false);

            var creature = ExFactory.Player(GameMaster.World);
            var act = new WalkAct(GameMaster.World.Reference(creature), 0, 1);

            var formed = false;
            GameMaster.MessageLog.Hub.Subscribe<WallBumpMessage>(msg => formed = true);
            Assert.IsTrue(act.Execute().Execute().Resolved);
            Assert.IsTrue(formed);

            formed = false;
            Assert.IsTrue(act.Consume().Resolved);
        }

        [TestMethod]
        public void ActSuspensionTest()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var player = ExFactory.Player(GameMaster.World);
            var act = new DieOnPurposeAct(player.Reference());

            Assert.IsFalse(act.Execute().Resolved);
            Assert.IsTrue(act.Suspended);
        }

        [TestMethod]
        public void CommandInjectionTest()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var player = ExFactory.Player(GameMaster.World);
            var act = new CommandInjectionAct();

            Assert.IsFalse(act.Execute().Resolved);
            Assert.IsTrue(act.Suspended);

            // Inject player instruction
            var walkAct = new WalkAct(player.Reference(), 1, 1);
            CommandInjectionAct.InjectedAct = walkAct;
            Assert.IsFalse(act.Suspended);

            // Assess
            var result = act.Execute();
            Assert.IsTrue(act.Resolved);
            Assert.IsFalse(act.Suspended);

            Assert.IsNull(CommandInjectionAct.InjectedAct);
            
            Assert.IsTrue(result.Execute().Resolved);
            Assert.IsFalse(result.Suspended);
        }
    }
}
