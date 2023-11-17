using Catalyster.Components;
using RogueSharp.DiceNotation;

namespace Catalyster.Items
{
    // TODO: Remove from use
    public abstract class Item
    {
        public float Fill { get; set; }
        public float Weight { get; set; }

        public RangedAttack ThrownAttack()
        {
            var range = Math.Max(6 - (int) Fill, 1);
            var attackFormula = new DiceExpression().Die(20).Constant(-(int)Fill);
            var damageFormula = new DiceExpression().Die(Math.Max((int)Weight * 2, 1));

            return new RangedAttack { AttackFormula = attackFormula, DamageFormula = damageFormula, Range = range };
        }
    }
}
