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
            Assert.IsNotNull(GameMaster.World);
            gm.Update();
        }

        [TestMethod]
        public void GameMasterTest2()
        {
            var map = new DungeonMap();
            var gm = new GameMaster(map);
            Assert.AreEqual(GameMaster.DungeonMap, map);
        }

        [TestMethod]
        public void GameMasterTest3()
        {
            var gm = new GameMaster();
            for (var i = 0; i < 10; i++)
            {
                ExFactory.SimpleCreature(GameMaster.World);
            }
            var player = ExFactory.Player(GameMaster.World);
            gm.Update();

            Assert.IsNotNull(gm.Control.Entity);
            Assert.AreEqual(player, gm.Control.Entity);
        }
    }
}
