using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public class MeleeAttackAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public EntityReference? Attacker;
        public EntityReference? Defender;
        public MeleeAttack? Attack;

        public MeleeAttackAct(EntityReference? attacker = null, EntityReference? defender = null, MeleeAttack? attack = null)
        {
            Attacker = attacker;
            Defender = defender;
            Attack = attack;
        }

        public IAct? Execute()
        {
            Entity atkr;
            Entity def;
            MeleeAttack att;
            if (!Defender.HasValue || !Attacker.HasValue)
            {
                //TODO: generate Moment(?)
                return null;
            }
            else if (!Attacker.Value.IsAlive() || !Defender.Value.IsAlive())
            {
                Console.Error.WriteLine($"Stale Entity Reference: {Attacker.Value} {Defender.Value}");
                return null;
            }
            else if (!Attack.HasValue)
            {
                (atkr, def) = (Attacker.Value.Entity, Defender.Value.Entity);
                if (ActionHelper.ResolveAttack(atkr, def))
                {
                    atkr.Get<Energy>().Points -= WiggleHelper.Wiggle(Cost, 0.1);
                    //return true;
                }
                return null;
            }
            else
            {
                (atkr, def, att) = (Attacker.Value.Entity, Defender.Value.Entity, Attack.Value);
                ActionHelper.ResolveMelee(att, def, atkr);
                ref var e = ref atkr.Get<Energy>();
                e.Points -= WiggleHelper.Wiggle(Cost, 0.1);
                //return true;
                return null;
            }
        }
    }
}
