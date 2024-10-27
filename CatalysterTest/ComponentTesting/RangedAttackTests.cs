using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;
using RogueSharp.DiceNotation;

namespace CatalysterTest.ComponentTesting
{
    [TestClass]
    public class RangedAttackTests
    {
        private Entity RangedMon(World world)
        {
            return world.Create(
                new Position { X = 0, Y = 0 },
                new Health { Max = 10, Points = 10 },
                new Defense { Class = 0 },
                new Energy { Max = 1000, Points = 1000, Regen = 1000 },
                new RangedAttack { Range = 2, AttackFormula = Dice.Parse("1d20+12"), DamageFormula = Dice.Parse("1d4") }
                );
        }

        [TestMethod]
        public void RangedAttackTest()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;
            // RangedMon written so they hit every time, but won't die in one hit.
            var attacker = RangedMon(world);
            var defender = RangedMon(world);

            var hp_0 = defender.Get<Health>().Points;

            Assert.IsTrue(ActionHelper.ResolveRanged(attacker.Get<RangedAttack>(), defender, attacker));

            var hp_1 = defender.Get<Health>().Points;

            Assert.IsTrue(hp_0 > hp_1);

            world.Dispose();
        }

        [TestMethod]
        public void RangedEnemyTest()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;
            // RangedMon written so they hit every time, but won't die in one hit.
            var attacker = RangedMon(world);
            var defender = RangedMon(world);

            var hp_0 = defender.Get<Health>().Points;

            Assert.IsTrue(ActionHelper.ResolveRanged(attacker.Get<RangedAttack>(), defender, attacker));

            var hp_1 = defender.Get<Health>().Points;

            Assert.IsTrue(hp_0 > hp_1);

            world.Dispose();
        }

        [TestCleanup]
        public void Cleanup()
        {
            GameMaster.Instance().Reset();
        }
    }
}
