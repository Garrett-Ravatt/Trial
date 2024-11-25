using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using CatalysterTest.TestUtils;

namespace CatalysterTest.Muck
{
    [TestClass]
    public class ArchWorldDisposeBugMuck
    {
        [TestMethod]
        private void BugRecreation1()
        {
            var worldA = World.Create();
            worldA.Create();
            World.Destroy(worldA);

            var worldB = World.Create();
            worldB.Create();
            World.Destroy(worldA);

            var c = ExFactory.SimpleCreature(worldB);
            Assert.ThrowsException<NullReferenceException>(() => c.Get<Token>());
        }
    }
}
