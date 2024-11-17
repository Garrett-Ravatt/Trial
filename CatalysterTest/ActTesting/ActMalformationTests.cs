

using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Components;
using CatalysterTest.TestUtils;

namespace CatalysterTest.ActTesting
{
    [TestClass]
    public class ActMalformationTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.Clear();
        }

        [TestMethod]
        public void MalformationTest1()
        {
            Assert.ThrowsException<Exception>(new WalkAct().Execute);
            Assert.ThrowsException<Exception>(new MeleeAttackAct().Execute);
            Assert.ThrowsException<Exception>(new ProbeAct().Execute);
            Assert.ThrowsException<Exception>(new WaitAct().Execute);
        }

        [TestMethod]
        public void MalformationTest2()
        {
            var gm = GameMaster.Instance();
            var c = ExFactory.SimpleCreature(gm.World);
            c.Set(new Position { X = 0, Y = 0 });
            gm.DungeonMap.SetCellProperties(1, 0, false, false);
            var act = new WalkAct(c.Reference(), 1, 0);
            Assert.ThrowsException<Exception>(act.Execute);
        }

        [TestMethod]
        public void MalformationTest3()
        {
            var gm = GameMaster.Instance();
        }
    }
}
