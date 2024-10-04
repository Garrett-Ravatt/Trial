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
            // Create attacker
            var attacker = ExFactory.SimpleCreature(world);
            // Create defender
            var defender = ExFactory.SimpleCreature(world);

            var initialHp = defender.Get<Health>().Points;

            // Perform directive
            var dir = new MeleeNearest { };

            Assert.IsTrue(dir.Enter(world.Reference(attacker)));
            Assert.IsTrue(initialHp > defender.Get<Health>().Points);

            World.Destroy(world);
        }
    }
}
