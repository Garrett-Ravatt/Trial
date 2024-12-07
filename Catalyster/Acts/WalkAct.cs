using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;

namespace Catalyster.Acts
{
    public class WalkAct : IAct
    {
        public int Cost { get; set; } = 1000;
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

        public EntityReference? Acting {  get; set; }

        public int? X;
        public int? Y;

        // Won't automatically attack if true
        public bool Passive;

        public WalkAct(EntityReference? e = null, int? x = null, int? y = null, bool passive = false)
        {
            Acting = e;
            X = x;
            Y = y;
            Passive = passive;
        }

        public IAct Execute()
        {
            if (!Acting.HasValue || !X.HasValue || !Y.HasValue)
                throw new Exception($"{this.GetType()} tried to execute with null values");

            var (entity, x, y) = (Acting.Value.Entity, X.Value, Y.Value);
            ref var stats = ref entity.Get<Stats>();

            ref var position = ref entity.Get<Position>();
            var newPos = new Position { X = position.X + x, Y = position.Y + y };
            
            var gm = GameMaster.Instance();

            if (newPos.X < 0 || newPos.Y < 0 ||
                newPos.X > gm.DungeonMap.Width || newPos.Y > gm.DungeonMap.Height)
            {
                Console.WriteLine($"{entity} trying to leave map");
                if (entity.Has<Player>())
                {
                    Resolved = true;
                    return this;
                }
                else
                    throw new Exception($"{entity} tried to leave map");
            }

            else if (gm.DungeonMap.IsWalkable(newPos.X, newPos.Y))
            {
                Entity? bumped = null;
                // An empty space or a an item
                if (SpatialHelper.ClearOrAssign(position.X + x, position.Y + y, ref bumped) ||
                    bumped.Value.Has<Item>())
                {
                    position = newPos;
                    // TODO: refer to movement speed
                    stats.Energy -= WiggleHelper.Wiggle(Cost, .1);
                    Resolved = true;
                    return this;
                }

                // A door
                // TODO: update walkable tiles instead of handling in walk act
                else if (bumped.Value.Has<Door>())
                {
                    ref var door = ref bumped.Value.Get<Door>();
                    if (door.state == DoorState.OPEN)
                    {
                        position = newPos;
                        // TODO: refer to movement speed
                        stats.Energy -= WiggleHelper.Wiggle(Cost, .1);
                    }
                    Resolved = true;
                    return this;
                }

                else if (bumped.Value.Has<Stats>() && !Passive) // ran into a creature; attack them
                {
                    // TODO: swap places with friendly creature
                    var attackAct = new MeleeAttackAct(Acting.Value, bumped.Value.Reference());
                    Resolved = true;
                    return attackAct;
                }
            }

            else if (entity.Has<Player>())
            {
                Resolved = true;
                return new ProbeAct(Acting, newPos.X, newPos.Y);
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
