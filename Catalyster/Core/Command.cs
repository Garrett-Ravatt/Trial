using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Acts;
using Catalyster.Acts.ItemManipulation;
using Catalyster.Components;
using Catalyster.Helpers;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.RAW;
using Arch.Relationships;

namespace Catalyster.Core
{
    public class Command
    {
        // The Ref being controlled
        public Entity? Entity;
        // Mapping of Inventory linearly to entity reference
        public List<EntityReference> InvList = new List<EntityReference>();
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
        public void Interact(bool forcePickup = false)
        {
            if (Entity == null)
            {
                Console.WriteLine("Command.Ref is null");
                return;
            }

            var player = Entity.Value;
            if (forcePickup)
                CommandInjectionAct.InjectedAct = new ItemCollectAct(player.Reference());
            else
                CommandInjectionAct.InjectedAct = new UseAct(player.Reference());
        }

        // Attempt to throw an item from inventory at a tile
        public bool Throw(int x, int y, int i)
        {
            if (Entity == null || !GameMaster.Instance().DungeonMap.IsInFov(x, y))
                return false;

            // if we would be out of range, try to update inventory list
            if (i >= InvList.Count)
                Inventory();

            var entity = Entity.Value;
            // TODO: refactor i for nested inventory
            var throwAct = new ThrowAct(GameMaster.Instance().World.Reference(entity), InvList[i], x, y);
            CommandInjectionAct.InjectedAct = throwAct;
            return true;
        }

        // Check if the player's turn is now over.
        public void CheckEnergy()
        {
            if (Entity == null)
                return;
            var entity = Entity.Value;
            if (entity.Get<Stats>().Energy <= 0)
                Entity = null;
        }

        // Pure Information Methods
        // TODO: Refactor pure information fetching into a different module?

        // player stats
        public Stats? Stats()
        {
            if (Entity == null)
                return null;
            Stats st;
            Entity.Value.TryGet<Stats>(out st);
            return st;
        }

        // Representation of Inventory used by UI
        public List<string> Inventory()
        {
            if (Entity == null)
                return new List<string>();

            var entity = Entity.Value;
            if (!entity.Has<Inventory>())
                return new List<string>();

            InvList = new List<EntityReference>();
            var list = new List<string>();
            foreach (EntityReference item in entity.Get<Inventory>().Items)
            {
                if (item.IsAlive())
                {
                    var e = item.Entity;
                    list.Add(ItemPropHelper.StringifyItem(e));
                    InvList.Add(item);
                    list = new List<string>(list.Concat(StringifyContent(e)));
                }
            }
            return list;
        }
        private List<string>StringifyContent(Entity container, int depth = 1)
        {
            var list = new List<string>();
            if (container.HasRelationship<Contains>())
                foreach ((var e, var r) in container.GetRelationships<Contains>())
                {
                    // TODO: depth as spaces
                    list.Add($"{new string('\t', depth)}└{ItemPropHelper.StringifyItem(e)}");
                    InvList.Add(e.Reference());
                    if (e.HasRelationship<Contains>())
                        list = new List<string>(list.Concat(StringifyContent(e, depth + 1)));
                }
            return list;
        }

        // Entity description of entity on a tile
        public EntityDefinition? Describe(int x, int y)
        {
            Entity? found = null;
            if (!SpatialHelper.ClearOrAssign(x, y, ref found))
            {
                var rid = found.Value.Get<Token>().RID;
                if (rid != null)
                    return GameMaster.Instance().Stats.Get(rid);
            }
            return null;
        }
    }
}
