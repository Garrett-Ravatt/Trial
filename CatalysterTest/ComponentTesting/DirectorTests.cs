﻿using Arch.Core;
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
        [TestCleanup]
        public void Cleanup()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void MonoBehaviorTest1()
        {
            GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var world = GameMaster.Instance().World;

            // Create MonoBehavior creature
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Stats { Breath = 10, Energy = 10 },
                new MonoBehavior { Directive = new RightMover { Cost = 1000 } }
                );

            // Simulate one turn
            world.Query(in new QueryDescription().WithAll<MonoBehavior, Stats>(), (Entity e, ref MonoBehavior behavior, ref Stats stats) =>
            {
                while (stats.Energy > 0)
                {
                    var act = behavior.Direct(e, world);
                    act.Execute();
                }
            });

            Assert.IsTrue(creature.Get<Position>().X >= 1);
            Assert.IsTrue(creature.Get<Position>().X <= 2);
        }

        [TestMethod]
        public void MonoBehaviorTest2()
        {
            GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var world = GameMaster.Instance().World;

            // Create MonoBehavior creature with higher move speed
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Stats { Breath = 10, Energy = 1500 },
                new MonoBehavior { Directive = new RightMover { Cost = 500 } }
                );

            // Simulate a turn (where creature should have two moves)
            world.Query(in new QueryDescription().WithAll<MonoBehavior, Stats>(), (Entity e, ref MonoBehavior behavior, ref Stats stats) =>
            {
                while (stats.Energy > 0)
                {
                    var act = behavior.Direct(e, world);
                    act.Execute();
                }
            });

            // Assert.AreEqual(2, creature.Get<Position>().X);
            Assert.IsTrue(creature.Get<Position>().X >= 2);
        }

        [TestMethod]
        public void InterfaceTest1()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;
            GameMaster.Instance().DungeonMap.Initialize(10, 10);
            GameMaster.Instance().DungeonMap.Clear();
            // Create MonoBehavior creature
            var creature = world.Create(
                new Position { X = 0, Y = 0 },
                new Stats { Breath = 10, Energy = 1000 },
                (IDirector) new MonoBehavior { Directive = new RightMover { Cost = 1000 } }
                );

            // Simulate one turn

            world.Query(in new QueryDescription().WithAll<IDirector, Stats>(), (Entity e, ref IDirector behavior, ref Stats stats) =>
            {
                while (stats.Energy > 0)
                {
                    var act = behavior.Direct(e, world);
                    if (act != null)
                        act.Execute();
                }
            });

            Assert.IsTrue(creature.Get<Position>().X > 0);
            Assert.IsTrue(creature.Get<Position>().X <= 2);
        }
        
        [TestMethod]
        public void CrazedHunterTest1()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(30, 30);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var world = GameMaster.Instance().World;

            var creature = ExFactory.SimpleCreature(world);
            creature.Set<IDirector>(new CrazedHunter { });
            creature.Set<Position>(new Position { X=3, Y=3 });

            var player = ExFactory.Player(world);
            player.Set<Position>(new Position { X=1, Y=1 });

            var initialDist = SpatialHelper.LazyDist(
                creature.Get<Position>(),
                player.Get<Position>());

            GameMaster.Instance().DungeonMap.UpdateFieldOfView(GameMaster.Instance().World);
            gm.Update();
            gm.Command.Wait();
            gm.Update();

            var finalDist = SpatialHelper.LazyDist(
                creature.Get<Position>(),
                player.Get<Position>());

            Assert.IsTrue(initialDist > finalDist);
        }

        [TestMethod]
        public void PlayerDirectorTest1()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(30, 30);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var world = GameMaster.Instance().World;

            var player = ExFactory.SimpleCreature(world);
            player.Set<IDirector>(new PlayerDirector { });

            var x = player.Get<Position>().X;

            GameMaster.Instance().DungeonMap.UpdateFieldOfView(GameMaster.Instance().World);
            gm.Update();

            CommandInjectionAct.InjectedAct = new WalkAct(player.Reference(), x: 1, y: 0);
            gm.Update();

            Assert.IsTrue(x < player.Get<Position>().X);
        }
    }
}
