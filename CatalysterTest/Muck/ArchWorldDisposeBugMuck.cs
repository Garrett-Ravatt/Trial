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
            var e0 = worldB.Create(0);
            World.Destroy(worldA);
            var e1 = worldB.Create(0);

            Assert.ThrowsException<NullReferenceException>(() => e0.Get<int>());
            Assert.ThrowsException<NullReferenceException>(() => e1.Get<int>());
        }
    }
}
