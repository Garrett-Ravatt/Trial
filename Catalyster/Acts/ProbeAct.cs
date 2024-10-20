using Arch.Core;
using Catalyster.Components;
using Catalyster.Interfaces;
using Catalyster.Messages;
using Arch.Core.Extensions;

namespace Catalyster.Acts
{
    public class ProbeAct : IAct
    {
        public int Cost { get; set; } = 200;
        public bool Resolved { get; set; } = false;
        public bool Suspended {  get; set; } = false;

        public EntityReference? EntityRef;
        public int? X, Y;
        public ProbeAct(EntityReference? entityReference, int? x, int? y)
        {
            EntityRef = entityReference;
            X = x;
            Y = y;
        }
        public IAct Execute()
        {
            // TODO: Throw Malformed
            if (!EntityRef.HasValue || !X.HasValue || !Y.HasValue) return this;
            var (entity, x, y) = (EntityRef.Value.Entity, X.Value, Y.Value);
            ref var energy = ref entity.Get<Energy>();

            //TODO: check depth of the wall
            if (!GameMaster.DungeonMap.IsWalkable(x, y))
            {
                GameMaster.MessageLog.Hub.Publish(new WallBumpMessage(this, x, y, entity.Reference()));
                energy.Points -= Cost;
                Resolved = true;
            }
            //TODO: else probe entities on the tile

            return this;
        }
    }
}
