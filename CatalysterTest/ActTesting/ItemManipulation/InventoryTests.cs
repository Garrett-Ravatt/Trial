using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Acts.ItemManipulation;
using CatalysterTest.TestUtils;
using Inventory = Catalyster.Items.Inventory;

namespace CatalysterTest.ActTesting.ItemManipulation
{
    [TestClass]
    public class InventoryTests
    {
        GameMaster GM;
        Entity Player;

        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
            
            GM = GameMaster.Instance();
            Player = ExFactory.Player(GM.World);
        }

        [TestMethod]
        public void PickupTest()
        {
            Assert.IsTrue(Player.Get<Inventory>().Items.Count == 0);

            ExFactory.BlackPowder(GM.World);
            
            var act = new ItemCollectAct(Player.Reference());
            act.Execute();
            Assert.IsTrue(Player.Get<Inventory>().Items.Count == 1);
        }

        [TestMethod]
        public void DropTest()
        {
            // Pickup Item
            ExFactory.BlackPowder(GM.World);
            new ItemCollectAct(Player.Reference()).Execute();

            // Drop Item
            var act = new DropItemAct(Player.Reference(), i:0);
            act.Execute();

            Assert.IsTrue(Player.Get<Inventory>().Items.Count == 0);
        }
    }
}
