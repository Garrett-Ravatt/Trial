using Arch.Core;
using Catalyster.Core;

namespace CatalysterTest
{
    [TestClass]
    public class ControlTests
    {
        [TestMethod]
        public void ControlTest1()
        {
            var world = World.Create();
            var control = new Control();
            control.Entity = ExFactory.Player(world);

            World.Destroy(world);
        }

        [TestMethod]
        public void ControlTest2()
        {
            var world = World.Create();
            var control = new Control();
            var order = new TurnOrder();

            var player = ExFactory.Player(world);
            control.Entity = order.Update(world);

            Assert.AreEqual(player, control.Entity);

            World.Destroy(world);
        }
    }
}
