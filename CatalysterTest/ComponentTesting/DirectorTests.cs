using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Interfaces;

namespace CatalysterTest.ComponentTests
{
    [TestClass]
    public class DirectorTests
    {
        [TestMethod]
        public void MonoBehaviorTest1()
        {
            var world = World.Create();
            // Create MonoBehavior creature
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MonoBehavior { Directive = new RightMover { Cost = 1000 } }
                );

            // Simulate one turn
            world.Query(in new QueryDescription().WithAll<MonoBehavior>(), (Entity e, ref MonoBehavior behavior) =>
            {
                behavior.Direct(e, world);
            });

            Assert.AreEqual(1, creature.Get<Position>().X);

            World.Destroy(world);
        }

        [TestMethod]
        public void MonoBehaviorTest2()
        {
            var world = World.Create();
            // Create MonoBehavior creature with higher move speed
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MonoBehavior { Directive = new RightMover { Cost = 500 } }
                );

            // Simulate a turn (where creature should have two moves)
            world.Query(in new QueryDescription().WithAll<MonoBehavior>(), (Entity e, ref MonoBehavior behavior) =>
            {
                behavior.Direct(e, world);
            });

            Assert.AreEqual(2, creature.Get<Position>().X);

            World.Destroy(world);
        }

        [TestMethod]
        public void InterfaceTest1()
        {
            var world = World.Create();
            // Create MonoBehavior creature
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                (IDirector) new MonoBehavior { Directive = new RightMover { Cost = 1000 } }
                );

            // Simulate one turn
            world.Query(in new QueryDescription().WithAll<IDirector>(), (Entity e, ref IDirector behavior) =>
            {
                behavior.Direct(e, world);
            });

            Assert.AreEqual(1, creature.Get<Position>().X);

            World.Destroy(world);
        }
    }
}
