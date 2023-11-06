using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Items;

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

                    else // Alchymer ran into a creature
                    {
                        ActionHelper.ResolveAttack(entity, bumped.Value);
                        // TODO: refer to attack cost
                        energy.Points -= WiggleHelper.Wiggle(1000, .1);
                    }
                }

                else // Alchymer ran into a wall.
                {
                    // TODO: Rip hunks of rock out of the wall.
                    GameMaster.MessageLog.Add("You bump into the wall. You fool.");
                }

                EndAction(energy.Points);
            }
            else
            {
                Console.WriteLine("Command.Entity is null");
            }
        }

        public void Throw(int x, int y, int i)
        {
            if (Entity == null)
                return;

            var entity = Entity.Value;

            ref var energy = ref entity.Get<Energy>();

            var item = entity.Get<Inventory>().Items[i];
            if (item == null)
            {
                Console.WriteLine($"Invalid item index {i} was selected");
                return;
            }

            // Check for a target.
            Entity? bumped = null;
            if (!SpatialHelper.ClearOrAssign(x, y, ref bumped))
            {
                energy.Points -= WiggleHelper.Wiggle(1000, .1);

                // Resolve an attack attempt
                ActionHelper.ResolveRanged(item.ThrownAttack(), bumped.Value);
            }

            EndAction(energy.Points);
        }

        // Check if the player's turn is now over.
        private void EndAction(int points)
        {
            if (points <= 0)
                Entity = null;
        }

        // A method used by UI
        public List<string> Inventory()
        {
            if (Entity == null)
                return new List<string>();
            var entity = Entity.Value;

            var list = new List<string>();
            foreach (Item item in entity.Get<Inventory>().Items)
            {
                list.Add(item.ToString());
            }
            return list;
        }
    }
}
