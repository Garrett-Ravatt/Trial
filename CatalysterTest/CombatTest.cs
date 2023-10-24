using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System;

using System.ComponentModel.DataAnnotations;

using Arch.Core;
using RogueSharp.DiceNotation;

using Catalyster.Interfaces;
using Catalyster.Components;
using Catalyster.Helpers;
using Arch.Core.Utils;
using Arch.Core.Extensions;

namespace CatalysterTest
{

    [TestClass]
    public class MeleeEntityTests
    {
        // Can create a simple creature
        [TestMethod]
        public void TestCreature()
        {
            var world = World.Create();
            // ExFactory makes our monsters for us.
            ExFactory.SimpleCreature(world);
            World.Destroy(world);
        }
    }

    [TestClass]
    public class ActionSystemTests
    {
        [TestMethod]
        public void TestAttackWorks()
        {
            var world = World.Create();
            // ActionHelper returns true if the attack lands.
            Assert.IsTrue(ActionHelper.ResolveAttack(ExFactory.SimpleCreature(world), ExFactory.SimpleCreature(world)));
            World.Destroy(world);
        }

        // Successful attack reduces hp
        [TestMethod]
        public void TestAttackDamage()
        {
            var world = World.Create();

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