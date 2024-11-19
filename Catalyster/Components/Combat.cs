using RogueSharp.DiceNotation;

namespace Catalyster.Components
{
    // Creature stats
    public struct Stats {
        // Consistent stats
        public int Blood;
        public int Body;
        public int Breath;

        // Stats that change quick
        public int HP;
        public int Energy;
    };

    // Turn Energy
    //public struct Energy { public int Max, Points; };

    // Form and damage for a single attack. An Ref can have many.
    public struct MeleeAttack { public DiceExpression AttackFormula, DamageFormula; };

    // Range, then form and damage for a single ranged attack.
    public struct RangedAttack { public int Range; public DiceExpression AttackFormula, DamageFormula; };

    // Range and damage for a single detonation
    public struct Detonation { public int Range; public DiceExpression DamageFormula;  };
}
