using Arch.Core;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.Components;
using Catalyster.Helpers;
using Arch.Relationships;

namespace CatalysterTest.ComponentTesting
{
    [TestClass]
    public class ItemEntityTests
    {
        [TestMethod]
        public void ItemEntityTest1()
        {
            var world = World.Create();

            Inventory inv = new Inventory();
            inv.Items = new List<EntityReference>
            {
                world.Create(new Item { Fill = 2f, Weight = 1f }).Reference(),
                world.Create(new Item { Fill = 1f, Weight = 2f }).Reference()
            };

            inv.CalculateCapacity();

            Assert.AreEqual(3, inv.Fill);
            Assert.AreEqual(3, inv.Weight);

            world.Dispose();
        }

        [TestMethod]
        public void ItemEntityTest2()
        {
            var world = World.Create();

            var bottle = world.Create(
                new Item {Fill = 2f, Weight = 1f},
                new Container { FillCap = 2f }
                );

            var dust = world.Create(
                new Item { Fill = 1f, Weight = 1f },
                new Explosive { }
                );

            ItemPropHelper.Contain(bottle, dust);

            Assert.IsTrue(bottle.HasRelationship<Contains>(dust));

            world.Dispose();
        }

        [TestMethod]
        public void ItemEntityTest3()
        {
            var world = World.Create();

            var bottle = world.Create(
                new Container { FillCap = 1f, Filled = 0f },
                new Item { Fill = 1f, Weight = 0.5f });

            var box = world.Create(
                new Container { FillCap = 10f, Filled = 0f },
                new Item { Fill = 10f, Weight = 3f });

            var dust = world.Create(new Item { Fill = 1f, Weight = 2f });

            Assert.IsTrue(ItemPropHelper.Contain(bottle, dust));
            Assert.AreEqual(2.5f, bottle.Get<Item>().Weight);
            Assert.AreEqual(1f, bottle.Get<Container>().Filled);

            Assert.IsTrue(ItemPropHelper.ReContain(bottle, box, dust));
            Assert.AreEqual(0.5f, bottle.Get<Item>().Weight);
            Assert.AreEqual(0f, bottle.Get<Container>().Filled);
            Assert.AreEqual(5f, box.Get<Item>().Weight);
            Assert.AreEqual(1f, box.Get<Container>().Filled);

            world.Dispose();
        }
    }
}
