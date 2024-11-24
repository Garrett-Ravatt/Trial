using Catalyster;
using CatalysterTest.TestUtils;
using Catalyster.Helpers;
using Catalyster.Components;

namespace CatalysterTest.HelperTesting
{
    [TestClass]
    public class QueryHelperTest
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void QueryHelperTest1()
        {
            GameMaster.Instance();
            ExFactory.Player(GameMaster.Instance().World);
            ExFactory.Player(GameMaster.Instance().World);
            Assert.AreEqual(2, QueryHelper.ListByComponent<Player>().Count);
        }
    }
}
