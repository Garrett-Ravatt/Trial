using Arch.Core;
using Catalyster.Core;
using Catalyster.Models;
using Catalyster.Components;
using CatalysterTest.TestUtils;
using Arch.Core.Extensions;
using Arch.CommandBuffer;

namespace CatalysterTest.ModelTesting
{
    [TestClass]
    public class EncounterGenTests
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
            public override void AddOn(CommandBuffer buffer, Entity entity)
            {
                buffer.Add(in entity, new Monster { });
            }
        }

        private struct Plant { }
        private class POIPlant : POIOverwrite
        {
            public POIPlant(double p = 1) : base(p) { }
            public override void AddOn(CommandBuffer buffer, Entity entity)
            {
                buffer.Add(in entity, new Plant { });
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
            });

            Assert.AreEqual(map.Rooms.Count, otherCount);
        }
    }
}
