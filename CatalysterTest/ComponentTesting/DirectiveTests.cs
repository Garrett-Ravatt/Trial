using Catalyster.Components;
using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using RogueSharp.DiceNotation;
using Catalyster.Helpers;
using CatalysterTest.TestUtils;

namespace CatalysterTest.ComponentTests
{
    [TestClass]
    public class DirectiveTests
    {
        [TestMethod]
        public void DirectiveTest1()
        {
            var world = World.Create();

            var e = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 100, Points = 100, Regen = 0 }
                );

            // Perform directive
            var dir = new RightMover { Cost = 100 };
            dir.Enter(e, world);

            Assert.AreEqual(1, e.Get<Position>().X);
            Assert.AreEqual(0, e.Get<Energy>().Points);

            World.Destroy(world);
        }

        [TestMethod]
        public void DirectiveTest2()
        {
            var world = World.Create();
            // Create attacker
            var attacker = ExFactory.SimpleCreature(world);
            // Create defender
            var defender = ExFactory.SimpleCreature(world);

            var initialHp = defender.Get<Health>().Points;

            // Perform directive
            var dir = new MeleeNearest { };

            Assert.IsTrue(dir.Enter(attacker, world));
            Assert.IsTrue(initialHp > defender.Get<Health>().Points);

            World.Destroy(world);
        }
    }
}
