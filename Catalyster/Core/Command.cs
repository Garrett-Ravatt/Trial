using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;

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
                // TODO: make sure entity can move there
                position.X+= X;
                position.Y+= Y;

                energy.Points -= 1000;

                EndAction(energy.Points);
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
