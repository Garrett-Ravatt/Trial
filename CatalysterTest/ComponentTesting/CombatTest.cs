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
            var world = GameMaster.World;
            // ExFactory makes our monsters for us.
            ExFactory.SimpleCreature(world);
            World.Destroy(world);
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
            var world = GameMaster.World;
            // ActionHelper returns true if the attack lands.
            Assert.IsTrue(ActionHelper.ResolveAttack(ExFactory.SimpleCreature(world), ExFactory.SimpleCreature(world)));
            World.Destroy(world);
        }

        // Successful attack reduces hp
        [TestMethod]
        public void TestAttackDamage()
        {
            GameMaster.Instance();
            var world = GameMaster.World;

            // Using ExFactory to make simple creatures.
            // Defense Class is always 0, so attacks always hit.
            var att = ExFactory.SimpleCreature(world);
            var def = ExFactory.SimpleCreature(world);

            var initialHP = def.Get<Health>().Points;

            // Do Attack
            ActionHelper.ResolveAttack(att, def);

            // Check hp difference
            Assert.IsTrue(initialHP > def.Get<Health>().Points);

            World.Destroy(world);
        }
    }
}