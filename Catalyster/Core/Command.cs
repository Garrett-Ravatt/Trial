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

        // yield control away.
        public void Wait()
        {
            Entity = null;
        }

        // try to go somewhere.
        public void Move(int X, int Y)
        {
            // TODO: Perform as Directive, or share code with a movement Directive using a Helper
            if (Entity != null)
            {
                var entity = Entity.Value;

                // Fail out if we can't perform the action.
                ref var energy = ref entity.Get<Energy>();
                if (energy.Points <= 0) return;

                ref var position = ref entity.Get<Position>();
                var newPos = new Position { X = position.X + X, Y = position.Y + Y };

                if (GameMaster.DungeonMap.IsWalkable(newPos.X, newPos.Y))
                {
                    Entity? bumped = null;
                    if (SpatialHelper.ClearOrAssign(position.X + X, position.Y + Y, ref bumped))
                    {
                        position = newPos;

                        energy.Points -= 1000;

                        EndAction(energy.Points);
                    }

                    else // Alchymer ran into a creature
                    {
                        // TODO: Attack. We need a reference to the entity in that square to do so.
                        //GameMaster.MessageLog.Add($"You try to attack {bumped.Value.Get<Token>().Name}!");
                        ActionHelper.ResolveAttack(entity, bumped.Value);
                        energy.Points -= 1000;
                        EndAction(energy.Points);
                    }
                }

                else // Alchymer ran into a wall.
                {
                    GameMaster.MessageLog.Add("You bump into the wall. You fool.");
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
