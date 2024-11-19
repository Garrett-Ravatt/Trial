using RogueSharp.DiceNotation;

namespace Catalyster.Components
{
    // Are you ok? This is the stat for that
    public struct  Health { public int Points, Max; };

    // Creature stats
    public struct Stats {
        public int Body;
        public int Blood;
        public int HP;
    };

    // Turn Energy
    public struct Energy { public int Max, Regen, Points; };

    // Form and damage for a single attack. An Ref can have many.
    public struct MeleeAttack { public DiceExpression AttackFormula, DamageFormula; };

    // Range, then form and damage for a single ranged attack.
    public struct RangedAttack { public int Range; public DiceExpression AttackFormula, DamageFormula; };

    // Range and damage for a single detonation
    public struct Detonation { public int Range; public DiceExpression DamageFormula;  };
}
