using Arch.Core;
using Catalyster.Core;
using Catalyster.Models;
using Catalyster.Components;
using CatalysterTest.TestUtils;
using Arch.Core.Extensions;

namespace CatalysterTest.ModelTesting
{
    [TestClass]
    public class WorldModelTests
    {
        private Model<DungeonMap> _mapModel = new Model<DungeonMap>()
                .Step(new InitializeMap(40, 40))
                .Step(new RoomGen(4, 3, 5)).Step(new CorridorGen())
                .Seed(0xbaba);

        [TestMethod] public void EncounterGenTest()
        {
            var map = _mapModel.Process(new DungeonMap());

            var world = World.Create();
            var model = new Model<World>()
                .Step(new POIGen(map));

            model.Process(world);

            var poiCount = 0;
            world.Query(in new QueryDescription().WithAll<POI, Position>(), (ref Position pos) =>
            {
                poiCount++;
                Assert.IsTrue(map.IsWalkable(pos.X, pos.Y));
            });

            // This may be refactored as generation has more variety.
            Assert.AreEqual(map.Rooms.Count, poiCount);

            world.Dispose();
        }

        private class POIMonster : POIOverwrite
        {
            public POIMonster(double p = 1) : base(p) { }
            public override Entity Make(World world)
            {
                return world.Create(new Monster { }, new Position { });
            }
        }

        private struct Plant { }
        private class POIPlant : POIOverwrite
        {
            public POIPlant(double p = 1) : base(p) { }
            public override Entity Make(World world)
            {
                return world.Create(new Plant { }, new Position { });
            }
        }

        [TestMethod]
        public void EncounterGenTest2()
        {
            var map = _mapModel.Process(new DungeonMap());

            var world = World.Create();
            var model = new Model<World>()
                .Step(new POIGen(map))
                .Step(new POIMonster(0.5))
                .Step(new POIPlant());

            model.Process(world);

            var poiCount = 0;
            world.Query(in new QueryDescription().WithAll<POI, Position>(), (ref Position pos) =>
            {
                poiCount++;
                Assert.IsTrue(map.IsWalkable(pos.X, pos.Y));
            });

            Assert.AreEqual(0, poiCount);

            var otherCount = 0;
            world.Query(in new QueryDescription().WithAny<Monster, Plant>().WithAll<Position>(), (ref Position pos) =>
            {
                otherCount++;
                Console.WriteLine($"{pos.X}, {pos.Y}");
            });

            Assert.AreEqual(map.Rooms.Count, otherCount);
            world.Dispose();
        }

        private class POIPlayer : POIOverwriteN
        {
            public POIPlayer(int n): base(n) { }
            public override Entity Make(World world)
            {
                return world.Create(new Position { }, new Player { });
            }
        }

        [TestMethod]
        public void GenerateNTest()
        {
            var map = _mapModel.Process(new DungeonMap());

            var world = World.Create();
            var model = new Model<World>()
                .Step(new POIGen(map))
                .Step(new POIPlayer(1));

            model.Process(world);

            var playerCount = 0;
            world.Query(in new QueryDescription().WithAll<Player, Position>(), (Entity e) => { playerCount++; });

            Assert.AreEqual(1, playerCount);

            world.Dispose(); // Only ok because we made this world
        }

        private struct Mineral { }
        private class MineralWrite : WallWrite
        {
            public MineralWrite(DungeonMap map) : base(map) { }
            public override Entity Make(World world)
            {
                return world.Create(new Mineral { }, new Position { });
            }
        }

        [TestMethod]
        public void WallWriteTest()
        {
            var map = _mapModel.Process(new DungeonMap());

            var world = World.Create();
            var model = new Model<World>()
                .Step(new MineralWrite(map));
            model.Process(world);

            var mCount = 0;
            world.Query(in new QueryDescription().WithAll<Mineral, Position>(), (ref Position pos) =>
            {
                //Assert.IsFalse(map.IsWalkable(pos.X, pos.Y));
                mCount++;
            });
            Assert.AreEqual(map.Rooms.Count, mCount);

            world.Dispose(); // Only ok because we made this world
        }

        private struct DoorBlock { }
        private class DoorBlockWrite : DoorwayWrite
        {
            public DoorBlockWrite(DungeonMap map) : base(map) { }
            public override Entity Make(World world)
            {
                return ExFactory.Door(world);
            }
        }

        [TestMethod]
        public void DoorwayWriteTest()
        {
            var map = _mapModel.Process(new DungeonMap());
            var model = new Model<World>()
                .Step(new DoorBlockWrite(map));
            var world = World.Create();
            world = model.Process(world);

            // num doors should be at least as many as rooms
            Assert.IsTrue(world.Size >= map.Rooms.Count);

            world.Dispose(); // ok because we created this world
        }
    }
}
