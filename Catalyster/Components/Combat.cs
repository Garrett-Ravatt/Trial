using RogueSharp.DiceNotation;

namespace Catalyster.Components
{    
    // Defense stats. Might contain saving throws or similar later
    public struct Defense { public int Class; };
    
    // Are you ok? This is the stat for that
    public struct  Health { public int Points, Max; };

    // Form and damage for a single attack. An Ref can have many.
    public struct MeleeAttack { public DiceExpression AttackFormula, DamageFormula; };

    // Range, then form and damage for a single ranged attack.
    public struct RangedAttack { public int Range; public DiceExpression AttackFormula, DamageFormula; };

    // Range and damage for a single detonation
    public struct Detonation { public int Range; public DiceExpression DamageFormula;  };

    // Turn Energy
    public struct Energy { public int Max, Regen, Points; };
}
