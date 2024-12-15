using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Inventory = Catalyster.Items.Inventory;

namespace Catalyster.Acts.ItemManipulation
{
    public class DropItemAct : IAct
    {
        public EntityReference? Acting { get; set; }
        public bool Resolved { get; set; } = false;
        public bool Suspended { get; set; } = false;

        // default cost of dropping an item
        public int Cost = 200;

        // item to be dropped
        public EntityReference? ItemRef;
        
        // not optional, but default to 0
        public int X, Y;

        public DropItemAct(EntityReference? acting = null, EntityReference? itemRef = null, int x = 0, int y = 0, int? i = null)
        {
            Acting = acting;

            if (itemRef != null)
                ItemRef = itemRef;
            
            else if (i != null && Acting.HasValue && Acting.Value.IsAlive()
                && Acting.Value.Entity.Has<Inventory>())
                ItemRef = Acting.Value.Entity.Get<Inventory>().Items[i.Value];

            (X, Y) = (x, y);
        }

        public IAct Execute()
        {
            if (!Acting.HasValue || !Acting.Value.IsAlive())
                throw new InvalidOperationException($"{this} executed with invalid acting entity");

            if (!ItemRef.HasValue || !ItemRef.Value.IsAlive())
                throw new InvalidOperationException($"{Acting.Value.Entity} tried to drop no item");

            var e = Acting.Value.Entity;
            ref var stats = ref e.Get<Stats>();
            ref var inv = ref e.Get<Inventory>();

            var itemRef = ItemRef.Value;
            var item = itemRef.Entity.Get<Item>();


            // Top Level inventory item
            // TODO: add as inventory method
            if (inv.Items.Contains(itemRef))
            {
                inv.Items.Remove(itemRef);
                inv.Fill -= item.Fill;
                inv.Weight -= item.Weight;

                itemRef.Entity.Add(e.Get<Position>());

                stats.Energy -= WiggleHelper.Wiggle(Cost, 0.1);
                Resolved = true;
                return this;
            }

            // TODO: Drop item from container instead of exception
            throw new NotImplementedException($"{this} doesn't implement this yet");
        }
    }
}
