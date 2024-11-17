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
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

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

        public IAct Execute()
        {
            // TODO: throw malformed
            if (!EntityRef.HasValue || !X.HasValue || !Y.HasValue)
                throw new Exception("WalkAct tried to execute with null values");
            var (entity, x, y) = (EntityRef.Value.Entity, X.Value, Y.Value);
            ref var energy = ref entity.Get<Energy>();

            ref var position = ref entity.Get<Position>();
            var newPos = new Position { X = position.X + x, Y = position.Y + y };

            if (GameMaster.Instance().DungeonMap.IsWalkable(newPos.X, newPos.Y))
            {
                Entity? bumped = null;
                if (SpatialHelper.ClearOrAssign(position.X + x, position.Y + y, ref bumped) ||
                    !bumped.Value.Has<Defense>())
                {
                    position = newPos;
                    // TODO: refer to movement speed
                    energy.Points -= WiggleHelper.Wiggle(Cost, .1);
                    Resolved = true;
                    return this;
                }

                else if (!Passive) // ran into a creature; attack them
                {
                    // TODO: swap places with friendly creature
                    var attackAct = new MeleeAttackAct(EntityRef.Value, bumped.Value.Reference());
                    Resolved = true;
                    return attackAct;
                }
            }

            else if (entity.Has<Player>())
            {
                Resolved = true;
                return new ProbeAct(EntityRef, newPos.X, newPos.Y);
            }

            else
            {
                // entity ran into a wall
                throw new Exception($"{entity} is trying to walk through a wall");
            }

            return this;
        }
    }
}
