using Arch.Core;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Hunks;
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

            var rock = world.Create(
                new Item { Fill = 1.1f, Weight = 1f }
                );

            // can fit the dust
            Assert.IsTrue(ItemPropHelper.Contain(bottle, dust));
            Assert.IsTrue(bottle.HasRelationship<Contains>(dust));

            // check weight
            Assert.AreEqual(2f, bottle.Get<Item>().Weight);

            // can't fit the rock
            Assert.IsFalse(ItemPropHelper.Contain(bottle, rock));
            Assert.IsFalse(bottle.HasRelationship<Contains>(rock));

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

        [TestMethod]
        public void ItemEntityTest4()
        {
            var world = World.Create();

            var bottle = world.Create(
                new Token { Name = "Bottle" },
                new Item { Fill = 2f, Weight = 1f },
                new Container { FillCap = 2f }
                );

            var dust = world.Create(
                new Item { Fill = 1f, Weight = 1f },
                new Explosive { }
                );

            ItemPropHelper.Contain(bottle, dust);

            var s = ItemPropHelper.StringifyItem(bottle);
            Console.WriteLine(s);
            Assert.AreEqual("Bottle, [ Fill: 2, Weight: 2 ] [ Space: 1 ]", s);

            world.Dispose();
        }

        [TestMethod]
        public void ItemEntityTest5()
        {
            var world = World.Create();

            //make bomb
            var bottle = world.Create(
            new Token { Name = "Bottle" },
            new Item { Fill = 2f, Weight = 1f },
            new Container { FillCap = 2f }
            );

            var dust = world.Create(
                new Item { Fill = 1f, Weight = 1f },
                new Explosive {
                    Potential = new IntHunk(new int[] { 1, 1 }),
                    Resistance = new IntHunk(new int[2])}
                );

            ItemPropHelper.Contain(bottle, dust);

            var dice = ItemPropHelper.BombOf(bottle);
            Console.WriteLine(dice);
            
            world.Dispose();
        }
    }
}
