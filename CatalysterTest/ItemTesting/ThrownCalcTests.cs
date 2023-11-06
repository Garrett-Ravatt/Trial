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
            var bottle = new Container(0.5f, 1f);
            RangedAttack rangedAttack = bottle.ThrownAttack();
            Assert.IsNotNull(rangedAttack);
        }
    }
}
