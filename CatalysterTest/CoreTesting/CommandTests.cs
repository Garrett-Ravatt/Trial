using Arch.Core;
using Catalyster.Core;
using Catalyster;
using Arch.Core.Extensions;
using Catalyster.Components;
using CatalysterTest.TestUtils;

namespace CatalysterTest.CoreTesting
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void CommandTest1()
        {
            var world = World.Create();
            var command = new Command();
            
            command.Move(0, 0);

            command.Entity = ExFactory.Player(world);
            command.Move(0, 0);

            World.Destroy(world);
        }

        [TestMethod]
        public void CommandTest2()
        {
            var world = World.Create();
            var command = new Command();
            var order = new TurnOrder();

            var player = ExFactory.Player(world);
            command.Entity = order.Update(world);

            Assert.AreEqual(player, command.Entity);

            World.Destroy(world);
        }

        [TestMethod]
        public void CommandTest3()
        {
            var gm = new GameMaster();
            var command = gm.Command;

            var player = ExFactory.Player(GameMaster.World);
            var iPos = player.Get<Position>();

            command.Entity = player;
            command.Move(0, 1);

            Assert.AreEqual(iPos.Y + 1, player.Get<Position>().Y);
        }

        [TestMethod]
        public void CommandTest4()
        {
            var gm = new GameMaster();
            var command = gm.Command;
            var world = GameMaster.World;

            var player = ExFactory.Player(world);

            command.Entity = player;
            command.Move(0, 1);
            command.Move(0, 1);

            Assert.IsTrue(player.Get<Energy>().Points <= 0);
            Assert.IsNull(command.Entity);

            world.Dispose();
        }
    }
}
