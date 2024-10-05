using Catalyster.Components;
using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using RogueSharp.DiceNotation;
using Catalyster.Helpers;
using CatalysterTest.TestUtils;
using Catalyster;

namespace CatalysterTest.ComponentTests
{
    [TestClass]
    public class DirectiveTests
    {
        [TestMethod]
        public void DirectiveTest1()
        {
            new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            var e = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 100, Points = 100, Regen = 0 }
                );

            // Perform directive
            var dir = new RightMover { Cost = 100 };
            dir.Enter(world.Reference(e));

            Assert.AreEqual(1, e.Get<Position>().X);
            Assert.IsTrue(e.Get<Energy>().Points <= 100);

            World.Destroy(world);
        }

        [TestMethod]
        public void DirectiveTest2()
        {
            new GameMaster();
            var world = GameMaster.World;
            // Create cat
            var attacker = ExFactory.SimpleCreature(world);
            // Create mouse
            var defender = ExFactory.SimpleCreature(world);

            var initialHp = defender.Get<Health>().Points;

            // Perform directive
            var dir = new MeleeNearest { };

            Assert.IsTrue(dir.Enter(world.Reference(attacker)));
            Assert.IsTrue(initialHp > defender.Get<Health>().Points);

            World.Destroy(world);
        }

        internal struct Mouse { }
        [TestMethod]
        public void DirectiveTest3()
        {
            new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;
            // Create cat
            var cat = ExFactory.SimpleCreature(world);
            cat.Set(new Position { X = 0, Y = 0 });
            cat.Set(new Faction { HostileDesc = new QueryDescription().WithAll<Mouse>() });
            // Create mouse
            var mouse = ExFactory.SimpleCreature(world);
            mouse.Set(new Position { X = 10, Y = 5 });
            mouse.Add<Mouse>();

            // Perform directive
            var dir = new PursueDir { };

            Assert.IsTrue(dir.Enter(world.Reference(cat)));
            Assert.AreEqual(cat.Get<Position>().Y, 1);
            Assert.AreEqual(cat.Get<Position>().X, 1);

            World.Destroy(world);
        }

        [TestMethod]
        public void DirectiveTest4()
        {
            new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;
            // Create cat
            var cat = ExFactory.SimpleCreature(world);
            cat.Set(new Position { X = 0, Y = 0 });
            cat.Set(new Faction { HostileDesc = new QueryDescription().WithAll<Mouse>() });
            // Create mouse
            var mouse = ExFactory.SimpleCreature(world);
            mouse.Set(new Position { X = 1, Y = 1 });
            mouse.Add<Mouse>();

            // Perform directive
            var dir = new PursueDir { };

            Assert.IsTrue(dir.Enter(world.Reference(cat)));
            Assert.AreEqual(cat.Get<Position>().Y, 0);
            Assert.AreEqual(cat.Get<Position>().X, 0);

            World.Destroy(world);
        }
    }
}
