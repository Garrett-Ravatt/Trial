using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Messages;

namespace Catalyster.Acts
{
    public class WalkAct : IAct
    {
        public int Cost { get; set; } = 1000;

        public EntityReference? EntityRef;

        public int? X;
        public int? Y;

        // Won't automatically attack if true
        public bool Passive;

        public WalkAct(EntityReference? e = null, int? x = null, int? y = 0, bool passive = false)
        {
            EntityRef = e;
            X = x;
            Y = y;
            Passive = passive;
        }

        public bool Execute()
        {
            if (!EntityRef.HasValue || !X.HasValue || !Y.HasValue) return false;
            var (entity, x, y) = (EntityRef.Value.Entity, X.Value, Y.Value);
            ref var energy = ref entity.Get<Energy>();

            ref var position = ref entity.Get<Position>();
            var newPos = new Position { X = position.X + x, Y = position.Y + y };

            if (GameMaster.DungeonMap.IsWalkable(newPos.X, newPos.Y))
            {
                Entity? bumped = null;
                if (SpatialHelper.ClearOrAssign(position.X + x, position.Y + y, ref bumped))
                {
                    position = newPos;
                    // TODO: refer to movement speed
                    energy.Points -= WiggleHelper.Wiggle(1000, .1);
                    return true;
                }

                else if (!Passive) // ran into a creature; attack them
                {
                    // TODO: swap places with friendly creature
                    var attackAct = new MeleeAttackAct(EntityRef.Value, bumped.Value.Reference());
                    return attackAct.Execute();
                }
            }

            else if (entity.Has<Player>())
            {
                GameMaster.MessageLog.Hub.Publish(new WallBumpMessage(this, x, y, entity.Reference()));
                // TODO: wall bump depth check
            }

            return true;
        }
    }
}
