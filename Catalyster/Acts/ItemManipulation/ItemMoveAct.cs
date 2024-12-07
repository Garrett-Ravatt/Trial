using Arch.Core;
using Arch.Relationships;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Components;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.Helpers;

namespace Catalyster.Acts.ItemManipulation
{
    public class ItemMoveAct : IAct
    {
        public EntityReference? Acting { get; set; }
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

        // default cost of dropping an item
        public int Cost = 200;

        // Container to take from
        public EntityReference? OldContainer;

        // Item to be dropped
        public EntityReference? ItemRef;

        // New location for entity
        public EntityReference? NewContainer;

        // TODO: Inventory index OR item entity reference

        public ItemMoveAct(EntityReference? acting = null, EntityReference? oldContainer = null,
            EntityReference? itemRef = null, EntityReference? newContainer = null)
        {
            Acting = acting;
            OldContainer = oldContainer;
            ItemRef = itemRef;
            NewContainer = newContainer;
        }

        public IAct Execute()
        {
            // Must have both references
            if (!Acting.HasValue || !Acting.Value.IsAlive() || !ItemRef.HasValue || !ItemRef.Value.IsAlive())
                throw new Exception($"{this} act executed without sufficient references");

            var (e, i) = (Acting.Value, ItemRef.Value);
            ref var stats = ref e.Entity.Get<Stats>();
            ref var item = ref i.Entity.Get<Item>();

            // Old container being used
            if (OldContainer.HasValue)
            {
                if (!OldContainer.Value.IsAlive())
                    throw new Exception($"Container entity {OldContainer.Value} is not alive");

                if (OldContainer.HasValue && !OldContainer.Value.Entity.Has<Container>())
                    throw new InvalidOperationException($"{this} must pass item and its old container");

                var o = OldContainer.Value;
                ref var oldContainer = ref o.Entity.Get<Container>();

                // C to C ; transfer from old container to new
                if (NewContainer.HasValue)
                {
                    var n = NewContainer.Value;
                    if (ItemPropHelper.ReContain(o, n, i))
                    {
                        stats.Energy -= WiggleHelper.Wiggle(Cost, 0.1);
                        Resolved = true;
                        return this;
                    }

                    // TODO: handle gently for player
                    throw new Exception($"{this} could not move {i} from {o} to {n}");
                }

                // C to I ; transfer from old container to inventory
                ref var inv = ref e.Entity.Get<Inventory>();
                if (inv.FillCapacity < inv.Fill + item.Fill)
                {
                    // TODO: handle gently for player
                    throw new Exception("item cannot fit");
                }

                if (!ItemPropHelper.Detain(o, i))
                    throw new Exception($"{this} removing an item from a container it isn't in");

                inv.Items.Add(i);
                inv.CalculateCapacity();

                stats.Energy -= WiggleHelper.Wiggle(Cost, 0.1);
                Resolved = true;
                return this;
            }

            // I to C ; transfer from inventory to container
            if (NewContainer != null)
            {
                var n = NewContainer.Value;
                if (ItemPropHelper.Contain(n, i))
                {
                    ref var inv = ref e.Entity.Get<Inventory>();
                    inv.Items.Remove(i);
                    inv.CalculateCapacity();
                    stats.Energy -= WiggleHelper.Wiggle(Cost, 0.1);
                    Resolved = true;
                    return this;
                }
                if (!e.Entity.Has<Player>())
                    throw new Exception($"{this} could not move {i} to {n}");
            }

            // Failure to move item
            throw new Exception($"{this} inventory manipulation act failed to move item");
        }
    }
}
