using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Acts;

namespace Catalyster.Components.Directives
{
    public struct MeleeNearest : IDirective
    {
        public MeleeNearest() { }
        public IAct Act(EntityReference entityref) { return new MeleeAttackAct(attacker: entityref); }
        public IAct? Enter(EntityReference entityref)
        {
            var (entity, world) = (entityref.Entity, World.Worlds[entityref.Entity.WorldId]);
            try
            {
                ref var stats = ref entity.Get<Stats>();
                // Fail out if we can't perform action
                if (stats.Energy <= 0) return null;

                var desc = new QueryDescription().WithAll<Position, Stats>();

                // prepare an attack action
                var act = (MeleeAttackAct)Act(entityref);
                var expended = false;
                // For now, attack the first entity.
                world.Query(in desc, (target) =>
                {
                    // TODO: check range
                    if (!expended && target != entity)
                    {
                        //expended = ActionHelper.ResolveAttack(entity, target);
                        act.Defender = target.Reference();
                        expended = true;
                    }
                });
                //TODO: return act;
                return act;
            }
            catch { return null; }
        }
    }
}
