using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Components;
using Catalyster.Components.Directives;
using Catalyster.Components.Directors;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using CatalysterTest.TestUtils;

namespace CatalysterTest.ComponentTests
{
    [TestClass]
    public class DirectorTests
    {
        [TestMethod]
        public void MonoBehaviorTest1()
        {
            new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            // Create MonoBehavior creature
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new MonoBehavior { Directive = new RightMover { Cost = 1000 } }
                );

            // Simulate one turn
            world.Query(in new QueryDescription().WithAll<MonoBehavior, Energy>(), (Entity e, ref MonoBehavior behavior, ref Energy energy) =>
            {
                while (energy.Points > 0)
                {
                    var act = behavior.Direct(e, world);
                    act.Execute();
                }
            });

            Assert.IsTrue(creature.Get<Position>().X >= 1);
            Assert.IsTrue(creature.Get<Position>().X <= 2);

            World.Destroy(world);
        }

        [TestMethod]
        public void MonoBehaviorTest2()
        {
            new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            // Create MonoBehavior creature with higher move speed
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Energy { Max = 1000, Points = 1500, Regen = 1000 },
                new MonoBehavior { Directive = new RightMover { Cost = 500 } }
                );

            // Simulate a turn (where creature should have two moves)
            world.Query(in new QueryDescription().WithAll<MonoBehavior, Energy>(), (Entity e, ref MonoBehavior behavior, ref Energy energy) =>
            {
                while (energy.Points > 0)
                {
                    var act = behavior.Direct(e, world);
                    act.Execute();
                }
            });

            // Assert.AreEqual(2, creature.Get<Position>().X);
            Assert.IsTrue(creature.Get<Position>().X >= 2);

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

            world.Query(in new QueryDescription().WithAll<IDirector, Energy>(), (Entity e, ref IDirector behavior, ref Energy energy) =>
            {
                while (energy.Points > 0)
                {
                    var act = behavior.Direct(e, world);
                    if (act != null)
                        act.Execute();
                }
            });

            Assert.IsTrue(creature.Get<Position>().X > 0);
            Assert.IsTrue(creature.Get<Position>().X <= 2);

            World.Destroy(world);
        }
        
        [TestMethod]
        public void CrazedHunterTest1()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(30, 30);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            var creature = ExFactory.SimpleCreature(world);
            creature.Set<IDirector>(new CrazedHunter { });
            creature.Set<Position>(new Position { X=3, Y=3 });

            var player = ExFactory.Player(world);
            player.Set<Position>(new Position { X=1, Y=1 });
            //player.AddOn<Sense>(new Sense { Range = 10 });

            var initialDist = SpatialHelper.LazyDist(
                creature.Get<Position>(),
                player.Get<Position>());

            GameMaster.DungeonMap.UpdateFieldOfView(GameMaster.World);
            gm.Update();
            gm.Command.Wait();
            gm.Update();

            var finalDist = SpatialHelper.LazyDist(
                creature.Get<Position>(),
                player.Get<Position>());

            Assert.IsTrue(initialDist > finalDist);

            World.Destroy(world);
        }

        [TestMethod]
        public void PlayerDirectorTest1()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(30, 30);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            var player = ExFactory.SimpleCreature(world);
            player.Set<IDirector>(new PlayerDirector { });

            var x = player.Get<Position>().X;

            PlayerDirector.nextAct = new WalkAct(player.Reference(), x:1, y:0);

            GameMaster.DungeonMap.UpdateFieldOfView(GameMaster.World);
            gm.Update();

            Assert.IsTrue(x < player.Get<Position>().X);

            World.Destroy(world);
        }
    }
}
