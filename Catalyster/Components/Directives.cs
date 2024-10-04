using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Helpers;

namespace Catalyster.Components
{
    public struct RightMover : IDirective
    {
        public int Cost { get; set; } = 1000;
        public RightMover() { }
        public bool Enter(EntityReference entityref)
        {
            var (entity, world) = (entityref.Entity, World.Worlds[entityref.Entity.WorldId]);
            try
            {
                // Fail out if we can't perform the action.
                ref var energy = ref entity.Get<Energy>();
                if (energy.Points <= 0) return false;

                ref var position = ref entity.Get<Position>();
                // TODO: make sure entity can move there
                position.X++;

                energy.Points -= WiggleHelper.Wiggle(Cost, .1);
                return true;
            }
            catch { return false; }
        }
    }

    public struct MeleeNearest : IDirective
    {
        public int Cost { get; set; } = 1000;
        public MeleeNearest() { }
        public bool Enter(EntityReference entityref)
        {
            var (entity, world) = (entityref.Entity, World.Worlds[entityref.Entity.WorldId]);
            try
            {
                ref var energy = ref entity.Get<Energy>();
                // Fail out if we can't perform action
                if (energy.Points <= 0) return false;

                var desc = new QueryDescription().WithAll<Position, Health, Defense>();

                var expended = false;
                // For now, attack the first entity.
                world.Query(in desc, (Entity target) =>
                {
                    // TODO: check range
                    if (!expended && target!=entity)
                    {
                        //ActionHelper.ResolveMelee(entity.Get<MeleeAttack>(), target);
                        ActionHelper.ResolveAttack(entity, target);

                        expended = true;
                    }
                });

                if (expended)
                {
                    energy.Points -= WiggleHelper.Wiggle(Cost, .1);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
    }
}
