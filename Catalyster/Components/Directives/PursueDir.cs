using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Helpers;
using Catalyster.Acts;

namespace Catalyster.Components.Directives
{
    public struct PursueDir : IDirective
    {
        private EntityReference? _markRef = null;
        public PursueDir() { }
        public IAct Act(EntityReference entityref) { return new WalkAct(entityref); }
        public IAct? Enter(EntityReference entityref)
        {
            var entity = entityref.Entity;

            Position target;
            if (!_markRef.HasValue || !_markRef.Value.IsAlive())
            {
                Faction faction;
                if (!entity.TryGet(out faction))
                    throw new Exception("Faction component not found.");
                _markRef = QueryHelper.ListByQuery(faction.HostileDesc).FirstOrDefault();
                if (_markRef == null)
                {
                    _markRef = null;
                    return null;
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
            return move;
        }
    }
}
