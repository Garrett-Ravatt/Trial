using Catalyster.Core;
using Catalyster.Models;
using RogueSharp;

namespace CatalysterTest
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestModel()
        {
            var model = new Model<DungeonMap>()
                .Step(new InitializeMap(40, 40))
                .Seed(11111); // Not necessary, just testing
            var map = model.Process(new DungeonMap());

            Assert.IsNotNull(map);
            Assert.AreEqual(40, map.Width);
        }
        
        [TestMethod]
        public void TestModel2()
        {
            var model = new Model<DungeonMap>()
                .Step(new InitializeMap(40, 40))
                .Step(new RoomGen(3, 8, 4))
                .Seed(111111);
            var map = model.Process(new DungeonMap());
            Assert.IsNotNull(map);

            int walkableCells = 0;
            foreach(Cell cell in map.GetAllCells())
                if ( cell.IsWalkable )
                    walkableCells++;

            Assert.IsTrue(walkableCells > 0);
            Assert.IsTrue(walkableCells >= 16);
            Assert.IsTrue(walkableCells <= 192);
        }
    }
}
