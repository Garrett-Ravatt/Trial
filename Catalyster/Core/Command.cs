using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Acts;
using Catalyster.Interfaces;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Messages;
using Inventory = Catalyster.Items.Inventory;

namespace Catalyster.Core
{
    public class Command
    {
        // The Ref being controlled
        public Entity? Entity;
        public Command() { }

        // yield control away.
        public void Wait()
        {
            if (Entity != null)
            {
                CommandInjectionAct.InjectedAct = new WaitAct(Entity.Value.Reference());
                Entity = null;
            }
        }

        // try to go somewhere.
        public void Move(int X, int Y)
        {
            if (Entity != null)
            {
                var e = Entity.Value;
                var walkAct = new WalkAct(GameMaster.Instance().World.Reference(e), X, Y);
                CommandInjectionAct.InjectedAct = walkAct;
                return;
            }
            else
            {
                Console.WriteLine("Command.Ref is null");
                return;
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
            // TODO: Refactor as an Injected Act
            GameMaster.Instance().World.Query(
                in new QueryDescription().WithAll<Token, Position, Item>(),
                (Entity entity, ref Token token, ref Position pos, ref Item item) =>
                {
                    // TODO: SpatialHash refactor point
                    // TODO: Check inventory capacity
                    if (SpatialHelper.LazyDist(position, pos) <= 1)
                    {
                        var entityRef = entity.Reference();
                        entity.Remove<Position>();
                        inv.Items.Add(entityRef);
                        GameMaster.Instance().MessageLog.Hub.Publish(new ItemCollectedMessage(player, player.Reference(), entityRef, item));
                        //GameMaster.Instance().MessageLog.Add($"{token.Name} Collected.");
                    }
                });
        }

        // Attempt to throw an item from inventory at a tile
        public bool Throw(int x, int y, int i)
        {
            if (Entity == null || !GameMaster.Instance().DungeonMap.IsInFov(x, y))
                return false;

            var entity = Entity.Value;
            var throwAct = new ThrowAct(GameMaster.Instance().World.Reference(entity), x, y, i);
            CommandInjectionAct.InjectedAct = throwAct;
            return true;
        }

        // Check if the player's turn is now over.
        public void CheckEnergy()
        {
            if (Entity == null)
                return;
            var entity = Entity.Value;
            if (entity.Get<Energy>().Points <= 0)
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
