using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Acts.ItemManipulation;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using CatalysterTest.TestUtils;
using System;
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

            var powder = ExFactory.BlackPowder(GM.World);
            
            var act = new ItemCollectAct(Player.Reference());
            act.Execute();
            Assert.IsTrue(Player.Get<Inventory>().Items.Count == 1);
            Assert.AreEqual(powder.Get<Item>().Fill, Player.Get<Inventory>().Fill);
            Assert.AreEqual(powder.Get<Item>().Weight, Player.Get<Inventory>().Weight);
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

            ref var inv = ref Player.Get<Inventory>();
            Assert.IsTrue(inv.Items.Count == 0);
            Assert.AreEqual(0, inv.Fill);
            
            // Item on ground
            var item = act.ItemRef.Value.Entity;
            Assert.AreEqual(Player.Get<Position>(), item.Get<Position>());
        }

        [TestMethod]
        public void MoveTest1()
        {
            var bomb = ExFactory.BasicBomb(GM.World).Reference();
            var relation = bomb.Entity.GetRelationships<Contains>().GetEnumerator();
            relation.MoveNext();
            var powder = relation.Current.Key.Reference();

            // Pickup Item
            new ItemCollectAct(Player.Reference()).Execute();

            // Container to Inventory
            var actCI = new ItemMoveAct(Player.Reference(), bomb, powder);
            actCI.Execute();
            Assert.IsTrue(actCI.Resolved);
            var inv = Player.Get<Inventory>();
            var cont = bomb.Entity.Get<Container>();
            var itemc = bomb.Entity.Get<Item>();
            var itemp = powder.Entity.Get<Item>();
            // inv ; two item references, fill of both items, weight of both items
            Assert.AreEqual(2, inv.Items.Count);
            Assert.AreEqual(itemc.Fill + itemp.Fill, inv.Fill);
            Assert.AreEqual(itemc.Weight + itemp.Weight, inv.Weight);
            // cont
            Assert.IsFalse(bomb.Entity.HasRelationship<Contains>(powder.Entity));
            Assert.AreEqual(0, cont.Filled);

            // Inventory to Container
            var actIC = new ItemMoveAct(Player.Reference(), null, powder, bomb);
            actIC.Execute();
            Assert.IsTrue(actIC.Resolved);
            inv = Player.Get<Inventory>();
            cont = bomb.Entity.Get<Container>();
            itemc = bomb.Entity.Get<Item>();
            // inv ; one item reference, fill of that item, weight of both items
            Assert.AreEqual(1, inv.Items.Count);
            Assert.AreEqual(itemc.Fill, inv.Fill);
            Assert.AreEqual(itemc.Weight, inv.Weight);
            // cont
            Assert.AreNotEqual(0, cont.Filled);
            Assert.IsTrue(bomb.Entity.HasRelationship<Contains>(powder));
        }

        [TestMethod]
        public void MoveTest2()
        {
            var bomb0 = ExFactory.BasicBomb(GM.World).Reference();
            var relation = bomb0.Entity.GetRelationships<Contains>().GetEnumerator();
            relation.MoveNext();
            var powder = relation.Current.Key.Reference();
            var bomb1 = ExFactory.BasicBomb(GM.World).Reference();

            // Container to Container
            var actCC = new ItemMoveAct(Player.Reference(), bomb0, powder, bomb1);
            actCC.Execute();
            Assert.IsTrue(actCC.Resolved);
            var inv = Player.Get<Inventory>();
            var cont = bomb0.Entity.Get<Container>();
            var itemc = bomb0.Entity.Get<Item>();
            var itemp = powder.Entity.Get<Item>();
        }
    }
}
