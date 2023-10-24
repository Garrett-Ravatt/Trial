using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Core;
using CatalysterTest.TestUtils;

namespace CatalysterTest.CoreTesting
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
            gm.Update();
            gm.Update();

            Assert.IsNotNull(gm.Command.Entity);
            Assert.AreEqual(player, gm.Command.Entity);
        }

        [TestMethod]
        public void GameMasterTest4()
        {
            var gm = new GameMaster();
            for (var i = 0; i < 10; i++)
            {
                ExFactory.SimpleCreature(GameMaster.World);
            }
            var player = ExFactory.Player(GameMaster.World);
            gm.Update();

            gm.Command.Move(0, 1);
            gm.Command.Move(0, 1);

            Assert.IsNull(gm.Command.Entity);
        }
    }
}
