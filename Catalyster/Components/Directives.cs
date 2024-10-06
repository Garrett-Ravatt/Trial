using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Helpers;
using Catalyster.Acts;

namespace Catalyster.Components
{
    public struct RightMover : IDirective
    {
        public int Cost { get; set; } = 1000;
        public bool Passive { get; set; } = false;
        public RightMover() { }
        public IAct Act(EntityReference entityref) { return new WalkAct(entityref, x: 1, y: 0, passive: Passive); }
        public bool Enter(EntityReference entityref)
        {
            var entity = entityref.Entity;
            // Fail out if we can't perform the action.
            ref var energy = ref entity.Get<Energy>();
            if (energy.Points <= 0) return false;

            var moveAct = (WalkAct) Act(entityref);
            return moveAct.Execute();
        }
    }

    public struct MeleeNearest : IDirective
    {
        public MeleeNearest() { }
        public IAct Act(EntityReference entityref) { return new MeleeAttackAct(attacker:entityref); }
        public bool Enter(EntityReference entityref)
        {
            var (entity, world) = (entityref.Entity, World.Worlds[entityref.Entity.WorldId]);
            try
            {
                ref var energy = ref entity.Get<Energy>();
                // Fail out if we can't perform action
                if (energy.Points <= 0) return false;

                var desc = new QueryDescription().WithAll<Position, Health, Defense>();

                // prepare an attack action
                var act = (MeleeAttackAct)Act(entityref);
                var expended = false;
                // For now, attack the first entity.
                world.Query(in desc, (Entity target) =>
                {
                    // TODO: check range
                    if (!expended && target!=entity)
                    {
                        //expended = ActionHelper.ResolveAttack(entity, target);
                        act.Defender = target.Reference();
                        expended = act.Execute();
                    }
                });
                return expended;
            }
            catch { return false; }
        }
    }

    public struct PursueDir : IDirective
    {
        private EntityReference? _markRef = null;
        public PursueDir() { }
        public IAct Act(EntityReference entityref) { return new WalkAct(entityref); }
        public bool Enter(EntityReference entityref)
        {
            var entity = entityref.Entity;

            Position target;
            if (!_markRef.HasValue || !_markRef.Value.IsAlive())
            {
                Faction faction;
                if (!entity.TryGet<Faction>(out faction))
                    throw new Exception("Faction component not found.");
                _markRef = QueryHelper.ListByQuery(faction.HostileDesc).FirstOrDefault();
                if (_markRef == null)
                {
                    _markRef = null;
                    return false;
                }
            }
            target = _markRef.Value.Entity.Get<Position>();
            var pos = entity.Get<Position>();

            var x = target.X - pos.X;
            x = Math.Clamp(x, -1, 1);
            var y = target.Y - pos.Y;
            y = Math.Clamp(y, -1, 1);

            var move = (WalkAct)Act(entityref);
            move.X = x;
            move.Y = y;
            return move.Execute();
        }
    }
}
