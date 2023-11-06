using Catalyster.Components;

namespace Catalyster.Items
{
    public class Inventory
    {
        public List<Item> Items;
        public float Fill,
            FillCapacity,
            Weight,
            WeightCapacity;

        public Inventory()
        {
            Items = new List<Item>();
        }

        public Inventory(List<Item> items)
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
