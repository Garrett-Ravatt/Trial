using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Components;
using Catalyster.Interfaces;
using CatalysterTest.TestUtils;

namespace CatalysterTest.ComponentTesting.Interactives
{
    [TestClass]
    public class InterActTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void InterActTest1()
        {
            var gm = GameMaster.Instance();
            var button = gm.World.Create(
                new Position { X = 1, Y = 0 },
                new InterAct { act = new DieOnPurposeAct(preConfirm: true) }
                );

            var player = ExFactory.SimpleCreature(gm.World).Reference();
            player.Entity.Set(new Position { X = 0, Y = 0 });

            IAct act = new UseAct(player);

            act = act.Execute();

            Assert.IsFalse(act.Resolved);
            Assert.AreEqual(typeof(DieOnPurposeAct), act.GetType());

            act = act.Consume();

            Assert.IsFalse(player.IsAlive());
        }
    }
}
