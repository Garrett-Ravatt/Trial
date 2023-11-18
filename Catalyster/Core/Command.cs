using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;
using Inventory = Catalyster.Items.Inventory;

namespace Catalyster.Core
{
    public class Command
    {
        // The Entity being controlled
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
                    GameMaster.MessageLog.Add("You bump into the wall. You fool.");
                }

                EndAction(energy.Points);
            }
            else
            {
                Console.WriteLine("Command.Entity is null");
            }
        }

        // TODO: implement
        // Collects items at the feet or interacts with something
        public void Interact()
        {
            if (Entity == null)
                return;
            var player = Entity.Value;
            if (!player.Has<Inventory, Position>())
                return;

            var position = player.Get<Position>();
            var inv = player.Get<Inventory>();

            // Look for items, collect them
            // TODO: Move somewhere else?
            GameMaster.World.Query(
                in new QueryDescription().WithAll<Token, Position, Item>(),
                (Entity entity, ref Token token, ref Position pos, ref Item item) =>
                {
                    // TODO: SpatialHash refactor point
                    if (SpatialHelper.LazyDist(position, pos) <= 1)
                    {
                        entity.Remove<Position>(); //NOTE: potentially unsafe.
                        inv.Items.Add(entity.Reference());
                        GameMaster.MessageLog.Add($"{token.Name} Collected.");
                    }
                });
        }

        // Attempt to throw an item from inventory at a tile
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
                ActionHelper.ResolveRanged(ItemPropHelper.ThrownAttack(item), bumped.Value);
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
            if (!entity.Has<Inventory>())
                return new List<string>();

            var list = new List<string>();
            foreach (EntityReference item in entity.Get<Inventory>().Items)
            {
                if (item.IsAlive())
                    list.Add(ItemPropHelper.StringifyItem(item.Entity));
            }
            return list;
        }
    }
}
