using Arch.Core;
using Catalyster.Components;
using Catalyster.Helpers;
using Arch.Core.Extensions;
using CatalysterTest.TestUtils;
using Catalyster;

namespace CatalysterTest.ComponentTests
{

    [TestClass]
    public class MeleeEntityTests
    {
        // Can create a simple creature
        [TestMethod]
        public void TestCreature()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;
            // ExFactory makes our monsters for us.
            ExFactory.SimpleCreature(world);
            GameMaster.Instance().Reset();
        }
    }

    [TestClass]
    public class ActionSystemTests
    {
        [TestCleanup]
        public void Cleanup()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void TestAttackWorks()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;
            // ActionHelper returns true if the attack lands.
            var c1 = ExFactory.SimpleCreature(world);
            var c2 = ExFactory.SimpleCreature(world);
            Assert.IsTrue(ActionHelper.ResolveAttack(c1, c2));
        }

        // Successful attack reduces hp
        [TestMethod]
        public void TestAttackDamage()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;

            // Using ExFactory to make simple creatures.
            // Body Class is always 0, so attacks always hit.
            var att = ExFactory.SimpleCreature(world);
            var def = ExFactory.SimpleCreature(world);

            ref var s = ref def.Get<Stats>(); s.Blood = 30; s.HP = 30;
            var initialHP = def.Get<Stats>().HP;

            // Do Attack
            ActionHelper.ResolveAttack(att, def);

            // Check hp difference
            Assert.IsTrue(initialHP > def.Get<Stats>().HP);
        }
    }
}