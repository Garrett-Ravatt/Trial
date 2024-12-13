﻿using CatalysterTest.TestUtils;
using Catalyster.Components;
using Catalyster.Acts;
using Catalyster.Interfaces;
using Catalyster;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Arch.Core;
using Catalyster.Messages;
using RogueSharp.DiceNotation;


namespace CatalysterTest.ActTesting
{
    [TestClass]
    public class ActTest
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.Clear();
        }

        [TestMethod]
        public void ActTest1()
        {
            var act1 = new WalkAct(x: 0, y: 1);
            var act2 = new WalkAct();
        }

        [TestMethod]
        public void ActTest2()
        {
            var creature = ExFactory.SimpleCreature(GameMaster.Instance().World);
            var act = new WalkAct(GameMaster.Instance().World.Reference(creature), 0, 1);

            var Y = creature.Get<Position>().Y;
            Assert.AreEqual(act, act.Execute());
            Assert.AreNotEqual(Y, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void ActTest3()
        {
            var gm = GameMaster.Instance();
            var world = GameMaster.Instance().World;

            var creature = ExFactory.SimpleCreature(GameMaster.Instance().World);
            var act = new WalkAct(world.Reference(creature), 0, 1);

            var Y = creature.Get<Position>().Y;
            Assert.IsTrue(act.Execute().Resolved);
            Assert.AreEqual(Y + 1, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void ActTest4()
        {
            var gm = GameMaster.Instance();
            var world = GameMaster.Instance().World;

            var creature = ExFactory.SimpleCreature(GameMaster.Instance().World);
            creature.Get<Stats>().Energy += 1000;
            var act = new WalkAct(creature.Reference(), 0, 1);

            var Y = creature.Get<Position>().Y;
            Assert.IsTrue(act.Execute().Resolved);
            Assert.AreEqual(Y + 1, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void ActTest5()
        {
            var gm = GameMaster.Instance();
            var world = GameMaster.Instance().World;

            var creature = ExFactory.SimpleCreature(GameMaster.Instance().World);
            creature.Get<Stats>().Energy += 1000;
            var act = new WalkAct(creature.Reference(), 0, 1);
            var item = ExFactory.BlackPowder(world);
            item.Set(new Position { X = 0, Y = 1 });

            var Y = creature.Get<Position>().Y;
            Assert.IsTrue(act.Execute().Resolved);
            Assert.AreEqual(Y + 1, creature.Get<Position>().Y);
        }

        [TestMethod]
        public void MeleeActTest1()
        {
            var gm = GameMaster.Instance();
            var world = GameMaster.Instance().World;

            var c1 = ExFactory.SimpleCreature(GameMaster.Instance().World);
            var c2 = ExFactory.SimpleCreature(GameMaster.Instance().World);

            var act = new MeleeAttackAct(c1.Reference(), c2.Reference());

            var formed = false;
            GameMaster.Instance().MessageLog.Hub.Subscribe<MeleeAttackMessage>(msg => formed = true);
            Assert.IsTrue(act.Execute().Resolved);
            Assert.IsTrue(formed);
        }

        [TestMethod]
        public void MeleeActTest2()
        {
            var gm = GameMaster.Instance();
            var c1 = ExFactory.SimpleCreature(GameMaster.Instance().World);
            c1.Get<Stats>().Body = 1;
            c1.Get<MeleeAttack>().AttackFormula = Dice.Parse("0");
            var c2 = ExFactory.SimpleCreature(GameMaster.Instance().World);
            c2.Get<Stats>().Body = 1;

            new MeleeAttackAct(c1.Reference(), c2.Reference()).Execute();

            Assert.AreNotEqual(c2.Get<Stats>().Blood, c2.Get<Stats>().HP);
        }

        [TestMethod]
        public void ThrowActTest1()
        {
            var gm = GameMaster.Instance();
            var world = GameMaster.Instance().World;

            var c1 = ExFactory.SimpleCreature(GameMaster.Instance().World);
            var item = ExFactory.BasicBomb(world);
            var items = new List<EntityReference> {
                item.Reference()
            };
            c1.Add(new Inventory(items));
            var act = new ThrowAct(world.Reference(c1), items[0], 0, 1);
            
            Assert.IsTrue(act.Execute().Resolved);
            Assert.IsTrue(item.Has<Position>());
        }

        [TestMethod]
        public void ProbeActTest1()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.SetCellProperties(0, 1, false, false);

            var creature = ExFactory.Player(GameMaster.Instance().World);
            var act = new ProbeAct(GameMaster.Instance().World.Reference(creature), 0, 1);

            var formed = false;
            GameMaster.Instance().MessageLog.Hub.Subscribe<WallBumpMessage>(msg => formed = true);
            Assert.IsTrue(act.Execute().Resolved);
            Assert.IsTrue(formed);
        }

        [TestMethod]
        public void AlternateActTest()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.SetCellProperties(0, 1, false, false);

            var creature = ExFactory.Player(GameMaster.Instance().World);
            var act = new WalkAct(GameMaster.Instance().World.Reference(creature), 0, 1);

            var formed = false;
            GameMaster.Instance().MessageLog.Hub.Subscribe<WallBumpMessage>(msg => formed = true);
            Assert.IsTrue(act.Execute().Execute().Resolved);
            Assert.IsTrue(formed);

            formed = false;
            Assert.IsTrue(act.Consume().Resolved);
        }

        [TestMethod]
        public void ActSuspensionTest()
        {
            var gm = GameMaster.Instance();

            var player = ExFactory.Player(GameMaster.Instance().World);
            var act = new DieOnPurposeAct(player.Reference());

            Assert.IsFalse(act.Execute().Resolved);
            Assert.IsTrue(act.Suspended);
        }

        [TestMethod]
        public void CommandInjectionTest()
        {
            var gm = GameMaster.Instance();

            var player = ExFactory.Player(GameMaster.Instance().World);
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

        [TestMethod]
        public void WaitActTest()
        {
            var gm = GameMaster.Instance();

            var player = ExFactory.Player(GameMaster.Instance().World);
            CommandInjectionAct.InjectedAct = new WaitAct(player.Reference());

            gm.Update();

            Assert.AreEqual(0, player.Get<Stats>().Energy);
        }

        [TestMethod]
        public void ThrowActTest()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {
            GameMaster.Instance().Reset();
        }
    }
}
