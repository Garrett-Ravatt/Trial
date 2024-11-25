using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Components.Directives;
using Catalyster.Components.Directors;
using Catalyster.Core;
using Catalyster.Interfaces;
using CatalysterTest.TestUtils;

namespace CatalysterTest.CoreTesting
{
    [TestClass]
    public class GameMasterTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void GameMasterTest1()
        {
            var gm = GameMaster.Instance();
            Assert.IsNotNull(gm);
            Assert.IsNotNull(GameMaster.Instance().World);
            gm.Update();
        }

        //[TestMethod]
        //public void GameMasterTest2()
        //{
        //    var map = new DungeonMap();
        //    var gm = new GameMaster(map);
        //    Assert.AreEqual(GameMaster.Instance().DungeonMap, map);
        //}

        [TestMethod]
        public void GameMasterTest3()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(10, 10);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            for (var i = 0; i < 10; i++)
            {
                ExFactory.SimpleCreature(GameMaster.Instance().World);
            }
            var player = ExFactory.Player(GameMaster.Instance().World);
            gm.Update();
            gm.Update();
            gm.Update();

            Assert.IsNotNull(gm.Command.Entity);
            Assert.AreEqual(player, gm.Command.Entity);
        }

        [TestMethod]
        public void GameMasterTest4()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            for (var i = 0; i < 10; i++)
            {
                ExFactory.SimpleCreature(GameMaster.Instance().World);
            }
            var p = ExFactory.Player(GameMaster.Instance().World);
            ref var stats = ref p.Get<Stats>();
            stats.Breath = 5;
            stats.Energy = 500;
            gm.Update();

            gm.Command.Move(0, 1);
            gm.Update();

            Assert.IsNull(gm.Command.Entity);
        }

        [TestMethod]
        public void GameMasterTest5()
        {
            var gm = GameMaster.Instance();
            var world = GameMaster.Instance().World;
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();

            var creatures = new List<EntityReference>();

            for (var i = 0; i < 10; i++)
            {
                var c = ExFactory.SimpleCreature(world);
                // The position must be updated before a director is added BECAUSE (I believe)
                // Once the director is added, the entity moves to a different chunk (?)
                c.Set(new Position { X = 1, Y = i + 1 });
                c.Add<IDirector>(new MonoBehavior { Directive = new RightMover { Cost = 1000 } });
                creatures.Add(c.Reference());
            }

            foreach (var entityref in creatures)
            {
                Assert.IsTrue(entityref.IsAlive());
                var entity = entityref.Entity;
                Console.WriteLine($"{entity.Get<Position>()}    {entity.Get<IDirector>()}");
                //Assert.IsTrue(entity.Get<Position>().X > 1);
            }

            var player = ExFactory.Player(world);
            player.Set(new Position { X=0, Y=0 });

            gm.Update();
            // Move down so we don't accidentally kill
            gm.Command.Move(0, 1);
            gm.Command.Move(0, 1);
            gm.Update();
            gm.Update();

            // Each entity should have moved at least 1 tile.
            foreach (var entityref in creatures)
            {
                Assert.IsTrue(entityref.IsAlive());
                var entity = entityref.Entity;
                Console.WriteLine($"{entity.Get<Position>()}    {entity.Get<IDirector>()}");
                //Assert.IsTrue(entity.Get<Position>().X > 1);
            }
        }

        [TestMethod]
        public void GameMasterTest6()
        {
            var gm = GameMaster.Instance();
            Assert.AreEqual(0, gm.Stats.Stats.Count);
        }
    }
}
