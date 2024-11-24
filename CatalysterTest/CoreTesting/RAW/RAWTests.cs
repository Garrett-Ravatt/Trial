using Catalyster.Core;

namespace CatalysterTest.CoreTesting.RAW
{
    // Tests RAW definitions without GameMaster

    [TestClass]
    public class RAWTests
    {
        private EntityStats stats;

        [TestInitialize]
        public void Initialize()
        {
            stats = new EntityStats();
        }

        [TestCleanup]
        public void Cleanup()
        {
            stats.World.Dispose();
        }

        [TestMethod]
        public void TestRaw()
        {

        }
    }
}
