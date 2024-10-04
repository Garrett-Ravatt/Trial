using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Components;
using Catalyster.Helpers;
using CommunityToolkit.HighPerformance.Extensions;
using Inventory = Catalyster.Items.Inventory;

namespace Catalyster.Core
{
    public class Command
    {
        // The EntityRef being controlled
        public Entity? Entity;
        public Command() { }

        // yield control away.
        public void Wait()
        {
            Entity = null;
        }

        // try to go somewhere.
        public bool Move(int X, int Y)
        {
            if (Entity != null)
            {
                var e = Entity.Value;
                var walkAct = new WalkAct(GameMaster.World.Reference(e), X, Y);
                var b = walkAct.Execute();
                CheckEnergy(e.Get<Energy>().Points);
                return b;
            }
            else
            {
                Console.WriteLine("Command.EntityRef is null");
                return false;
            }
        }

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
                    // TODO: Check inventory capacity
                    if (SpatialHelper.LazyDist(position, pos) <= 1)
                    {
                        entity.Remove<Position>(); //NOTE: potentially unsafe.
                        inv.Items.Add(entity.Reference());
                        GameMaster.MessageLog.Add($"{token.Name} Collected.");
                    }
                });
        }

        // Attempt to throw an item from inventory at a tile
        public bool Throw(int x, int y, int i)
        {
            if (Entity == null || !GameMaster.DungeonMap.IsInFov(x, y))
                return false;

            var entity = Entity.Value;
            var throwAct = new ThrowAct(GameMaster.World.Reference(entity), x, y, i);

            var didThrow = throwAct.Execute();
            CheckEnergy(entity.Get<Energy>().Points);
            return didThrow;
        }

        // Check if the player's turn is now over.
        private void CheckEnergy(int points)
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
