
using Arch.Core;
using CatalysterTest.TestUtils;

namespace CatalysterTest.ComponentTesting
{
    [TestClass]
    public class AggroTest
    {
        [TestMethod]
        public void FactionMatch()
        {
            var world = World.Create();

            var e = ExFactory.SimpleCreature(world);

            world.Dispose();
        }
    }
}
