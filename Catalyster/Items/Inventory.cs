using Catalyster.Components;
using Catalyster.Interfaces;

namespace Catalyster.Items
{
    public class Inventory
    {
        public List<IItem> Items;
        public float Fill,
            FillCapacity,
            Weight,
            WeightCapacity;

        public Inventory()
        {
            Items = new List<IItem>();
        }

        public Inventory(List<IItem> items)
        {
            Items = items;
            CalculateCapacity();
        }

        public void CalculateCapacity()
        {
            Fill = 0;
            Weight = 0;
            foreach (var item in Items)
            {
                if (item is Container)
                {
                    //CalculateCapacity(ref (Container)item);
                }
                Fill += item.Fill;
                Weight += item.Weight;
            }
        }
    }
}
