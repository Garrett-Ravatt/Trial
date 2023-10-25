using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;

namespace Catalyster.Core
{
    public class Command
    {
        // The Entity being controlled
        // TODO: use eventbus to set Entity instead
        public Entity? Entity;
        public Command() { }

        public void Move(int X, int Y)
        {
            // TODO: Perform as Directive, or share code with a movement Directive
            if (Entity != null)
            {
                var entity = Entity.Value;

                // Fail out if we can't perform the action.
                ref var energy = ref entity.Get<Energy>();
                if (energy.Points <= 0) return;

                ref var position = ref entity.Get<Position>();
                var newPos = new Position { X = position.X + X, Y = position.Y + Y };

                if (SpatialHelper.IsClear(new Position { X=position.X+X, Y=position.Y+Y}) 
                    && GameMaster.DungeonMap.IsWalkable(newPos.X, newPos.Y))
                {
                    position = newPos;

                    energy.Points -= 1000;

                    EndAction(energy.Points);
                }

                return;
            }
            else
            {
                Console.WriteLine("Command.Entity is null");
            }
        }

        // Check if the player's turn is now over.
        private void EndAction(int points)
        {
            if (points <= 0)
                Entity = null;
        }
    }
}
