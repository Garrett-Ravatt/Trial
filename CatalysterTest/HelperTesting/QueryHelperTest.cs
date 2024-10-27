using Catalyster;
using CatalysterTest.TestUtils;
using Catalyster.Helpers;
using Catalyster.Components;

namespace CatalysterTest.HelperTesting
{
    [TestClass]
    public class QueryHelperTest
    {
        [TestMethod]
        public void QueryHelperTest1()
        {
            GameMaster.Instance();
            ExFactory.Player(GameMaster.World);
            ExFactory.Player(GameMaster.World);
            Assert.AreEqual(2, QueryHelper.ListByComponent<Player>().Count);
        }
    }
}
