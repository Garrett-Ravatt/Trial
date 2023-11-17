using Catalyster.Components;
using Catalyster.Items;

namespace CatalysterTest.ItemTesting
{

    [TestClass]
    public class InventoryTest
    {
        // TODO: Refactor into Entity at ItemEntityTests
        //[TestMethod]
        //public void CapacityCalculation()
        //{
        //    Inventory inv = new Inventory();
        //    inv.Items = new List<Item>
        //    {
        //        new BasicItem { Fill = 2f, Weight = 1f },
        //        new BasicItem { Fill = 1f, Weight = 2f }
        //    };

        //    inv.CalculateCapacity();

        //    Assert.AreEqual(3, inv.Fill);
        //    Assert.AreEqual(3, inv.Weight);
        //}

        //[TestMethod]
        //public void CapacityContainerCalculation()
        //{
        //    var container = new Container(3f, 0.5f,
        //        new List<Item>
        //        {
        //            new Fluid{Fill = 2f, Weight= 1f},
        //        });

        //    Assert.AreEqual(1, container.Contents.Count);
        //    Assert.AreEqual(3f, container.Fill);

        //    Assert.AreEqual(2f, container.Filled);
        //    Assert.AreEqual(1.5f, container.Weight);
        //}
    }
}
