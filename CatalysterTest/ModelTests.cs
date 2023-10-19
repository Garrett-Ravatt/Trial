using Catalyster.Core;
using Catalyster.Models;

namespace CatalysterTest
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestModel()
        {
            var model = new Model<DungeonMap>()
                .Step(new InitializeMap(40, 40));
            var map = model.Process(new DungeonMap());

            Assert.IsNotNull(map);
            Assert.AreEqual(40, map.Width);
        }
    }
}
