using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Acts;
using Catalyster.Helpers;
using Catalyster.Interfaces;

namespace Catalyster.Components.Directors
{
    // Two brain cell hunter
    public struct CrazedHunter : IDirector
    {
        private EntityReference? _markRef = null;

        public CrazedHunter() { }

        public IAct? Direct(Entity entity, World world)
        {
            ref var pos = ref entity.Get<Position>();

            // does nothing if the player can't see it.
            if (!GameMaster.Instance().DungeonMap.IsInFov(pos.X, pos.Y))
            {
                //Console.WriteLine("They can't see me");
                return null;
            }

            if (_markRef == null)
            {
                Faction faction;
                if (!entity.TryGet(out faction))
                    throw new Exception("Faction component not found.");
                _markRef = QueryHelper.ListByQuery(faction.HostileDesc).FirstOrDefault();
            }

            // If I still can't find them, I give up.
            if (_markRef == null || !_markRef.Value.IsAlive())
                return null;

            var target = _markRef.Value.Entity.Get<Position>();

            if (SpatialHelper.LazyDist(pos, target) <= 1)
            {
                var att = new MeleeAttackAct(entity.Reference(), _markRef.Value);
                return att;
            }
            else
            {
                var x = target.X - pos.X;
                x = Math.Clamp(x, -1, 1);
                var y = target.Y - pos.Y;
                y = Math.Clamp(y, -1, 1);

                var act = new WalkAct(entity.Reference(), 0, 0);

                // TODO: refactor to static method elsewhere.
                if (GameMaster.Instance().DungeonMap.IsWalkable(pos.X + x, pos.Y + y))
                {
                    act.X = x;
                    act.Y = y;
                }
                else if (GameMaster.Instance().DungeonMap.IsWalkable(pos.X + x, pos.Y))
                {
                    act.X = x;
                }
                else if (GameMaster.Instance().DungeonMap.IsWalkable(pos.X, pos.Y + y))
                {
                    act.Y += y;
                }

                return act;
            }
        }
    }
}
