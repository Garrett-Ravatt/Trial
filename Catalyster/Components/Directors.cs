using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Helpers;
using Catalyster.Interfaces;

namespace Catalyster.Components
{
    // Does one thing over and over
    public struct MonoBehavior : IDirector
    {
        public IDirective Directive;
        public void Direct(Entity entity, World world)
        {
            // Run until Directive fails :)
            while (Directive.Enter(world.Reference(entity))) {}
        }
    }

    // Two brain cell hunter
    public struct CrazedHunter : IDirector
    {
        private EntityReference? _markRef = null;

        public CrazedHunter() { }

        public void Direct(Entity entity, World world)
        {
            ref var pos = ref entity.Get<Position>();

            // does nothing if the player can't see it.
            if (!GameMaster.DungeonMap.IsInFov(pos.X, pos.Y))
            {
                //Console.WriteLine("They can't see me");
                return;
            }

            if (_markRef == null)
            {
                Faction faction;
                if (!entity.TryGet<Faction>(out faction))
                    throw new Exception("Faction component not found.");
                _markRef = QueryHelper.ListByQuery(faction.HostileDesc).FirstOrDefault();
            }

            // If I still can't find them, I give up.
            if (_markRef == null || !_markRef.Value.IsAlive())
                return;

            var target = _markRef.Value.Entity.Get<Position>();

            ref var energy = ref entity.Get<Energy>();
            while (energy.Points > 0)
            {
                if (SpatialHelper.LazyDist(pos, target) <= 1)
                {
                    ActionHelper.ResolveAttack(entity, _markRef.Value.Entity);
                    energy.Points -= WiggleHelper.Wiggle(1000, .1); // TODO: replace with attack cost
                }
                else
                {
                    var x = target.X - pos.X;
                    x = Math.Clamp(x, -1, 1);
                    var y = target.Y - pos.Y;
                    y = Math.Clamp(y, -1, 1);
                    
                    // TODO: refactor to static method elsewhere.
                    if (GameMaster.DungeonMap.IsWalkable(pos.X+x, pos.Y+y))
                    {
                        pos.X += x;
                        pos.Y += y;
                    }
                    else if (GameMaster.DungeonMap.IsWalkable(pos.X+x, pos.Y))
                    {
                        pos.X += x;
                    }
                    else if (GameMaster.DungeonMap.IsWalkable(pos.X, pos.Y+y))
                    {
                        pos.Y += y;
                    }

                    energy.Points -= WiggleHelper.Wiggle(1000, .1); // TODO: replace with movement cost
                }
            }

        }
    }
}
