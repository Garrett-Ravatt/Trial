using Catalyster;
using Catalyster.Core;

namespace CatalysterTest
{
    [TestClass]
    public class GameMasterTests
    {
        [TestMethod]
        public void GameMasterTest1()
        {
            var gm = new GameMaster();
            Assert.IsNotNull(gm);
            Assert.IsNotNull(gm.World);
            gm.Update();
        }

        [TestMethod]
        public void GameMasterTest2()
        {
            var map = new DungeonMap();
            var gm = new GameMaster(map);
            Assert.AreEqual(gm.DungeonMap, map);
        }

        [TestMethod]
        public void GameMasterTest3()
        {
            var gm = new GameMaster();
            for (var i = 0; i < 10; i++)
            {
                ExFactory.SimpleCreature(gm.World);
            }
            var player = ExFactory.Player(gm.World);
            gm.Update();

            Assert.IsNotNull(gm.Control.Entity);
            Assert.AreEqual(player, gm.Control.Entity);
        }
    }
}
