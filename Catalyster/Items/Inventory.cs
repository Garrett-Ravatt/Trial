using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;

namespace Catalyster.Items
{
    public class Inventory
    {
        public List<EntityReference> Items;
        public float Fill,
            FillCapacity,
            Weight,
            WeightCapacity;

        public Inventory()
        {
            Items = new List<EntityReference>();
        }

        public Inventory(List<EntityReference> items)
        {
            Items = items;
            CalculateCapacity();
        }

        public void CalculateCapacity()
        {
            Fill = 0;
            Weight = 0;
            foreach (var r in Items)
            {
                if (!r.IsAlive())
                {
                    Console.WriteLine($"{r} is dead in inventory!");
                    break;
                }
                else if (r.Entity.Has<Components.Item>())
                {
                    var item = r.Entity.Get<Components.Item>();
                    Fill += item.Fill;
                    Weight += item.Weight;
                }
            }
        }
    }
}
