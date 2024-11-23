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

        public EntityReference? Acting { get; set; }
        public int? X, Y;
        public ProbeAct(EntityReference? entityReference = null, int? x = null, int? y = null)
        {
            Acting = entityReference;
            X = x;
            Y = y;
        }
        public IAct Execute()
        {
            if (!Acting.HasValue || !X.HasValue || !Y.HasValue)
                throw new Exception($"{this.GetType()} tried to execute with null values");

            var (entity, x, y) = (Acting.Value.Entity, X.Value, Y.Value);
            ref var stats = ref entity.Get<Stats>();

            //TODO: check depth of the wall
            if (!GameMaster.Instance().DungeonMap.IsWalkable(x, y))
            {
                GameMaster.Instance().MessageLog.Hub.Publish(new WallBumpMessage(this, x, y, entity.Reference()));
                stats.Energy -= Cost;
                Resolved = true;
            }
            //TODO: else probe entities on the tile

            return this;
        }
    }
}
