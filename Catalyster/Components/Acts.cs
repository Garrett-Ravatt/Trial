using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using System.Transactions;

namespace Catalyster.Components
{
    public class WalkAct : IAct
    {
        public int Cost { get; set; } = 1000;
        
        public int X;
        public int Y;

        public WalkAct(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public bool Enter(Entity entity, World world)
        {
            ref var energy = ref entity.Get<Energy>();

            ref var position = ref entity.Get<Position>();
            var newPos = new Position { X = position.X + X, Y = position.Y + Y };

            if (GameMaster.DungeonMap.IsWalkable(newPos.X, newPos.Y))
            {
                Entity? bumped = null;
                if (SpatialHelper.ClearOrAssign(position.X + X, position.Y + Y, ref bumped))
                {
                    position = newPos;
                    // TODO: refer to movement speed
                    energy.Points -= WiggleHelper.Wiggle(1000, .1);
                }

                else // ran into a creature
                {
                    ActionHelper.ResolveAttack(entity, bumped.Value);
                    // TODO: refer to attack cost
                    energy.Points -= WiggleHelper.Wiggle(1000, .1);
                }
            }

            else if (entity.Has<Player>()) {
                GameMaster.MessageLog.Add("You bump into the wall. You fool.");
            }

            return true;
        }
    }
}
