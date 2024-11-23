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
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

        public EntityReference? Acting { get; set; }
        public EntityReference? Defender;
        public MeleeAttack? Attack;

        public MeleeAttackAct(EntityReference? attacker = null, EntityReference? defender = null, MeleeAttack? attack = null)
        {
            Acting = attacker;
            Defender = defender;
            Attack = attack;
        }

        public IAct Execute()
        {
            Entity atkr;
            Entity def;
            MeleeAttack att;
            if (!Defender.HasValue || !Acting.HasValue)
            {
                throw new Exception($"{GetType()} tried to execute with unresolved entity references");
            }
            else if (!Acting.Value.IsAlive() || !Defender.Value.IsAlive())
            {
                throw new Exception($"{GetType()} Ran into Stale Entity Reference(s): {Acting.Value} {Defender.Value}");
            }
            else if (!Attack.HasValue)
            {
                (atkr, def) = (Acting.Value.Entity, Defender.Value.Entity);
                if (ActionHelper.ResolveAttack(atkr, def))
                {
                    atkr.Get<Stats>().Energy -= WiggleHelper.Wiggle(Cost, 0.1);
                    Resolved = true;
                }
                //TODO: generate error
            }
            else
            {
                (atkr, def, att) = (Acting.Value.Entity, Defender.Value.Entity, Attack.Value);
                ActionHelper.ResolveMelee(att, def, atkr);
                ref var e = ref atkr.Get<Stats>();
                e.Energy -= WiggleHelper.Wiggle(Cost, 0.1);
                Resolved = true;
            }
            return this;
        }
    }
}
