using Catalyster.Items;
using Catalyster.Components;

namespace CatalysterTest.ItemTesting
{
    [TestClass]
    public class ThrownCalcTests
    {
        [TestMethod]
        public void ThrownCalc()
        {
            var bottle = new Catalyster.Items.Container(0.5f, 1f);
            RangedAttack rangedAttack = bottle.ThrownAttack();
            Assert.IsNotNull(rangedAttack);
        }

        [TestMethod]
        public void ThrownCalc2()
        {
            var rand = new Random(0xdab);

            for (int i = 0; i < 10; i++)
            {
                var doubl = i + (float) rand.NextDouble();

                var item = new BasicItem { Fill = doubl, Weight = doubl };
                Assert.IsNotNull(item.ThrownAttack());
            }
        }
    }
}
