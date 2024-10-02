using CatalysterTest.TestUtils;
using Catalyster.Components;
using Catalyster;
using Arch.Core.Extensions;

namespace CatalysterTest.ActTesting
{
    [TestClass]
    public class ActTest
    {

        [TestMethod]
        public void ActTest1()
        {
            var act1 = new MoveAct(0, 1);
            var act2 = new MoveAct();
        }

        [TestMethod]
        public void ActTest2()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();

            var player = ExFactory.SimpleCreature(GameMaster.World);
            var act = new MoveAct(0, 1);

            var Y = player.Get<Position>().Y;

            Assert.IsTrue(act.Enter(player, GameMaster.World));
            Assert.AreNotEqual(Y, player.Get<Position>().Y);
        }
    }
}
