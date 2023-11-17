using Catalyster.Components;

namespace Catalyster.Items
{
    // TODO: All will be refactored into a helper for InvECS
    public class Container : Item
    {
        public float Fill { get; set; }
        public float Weight { get; set; }

        // TODO: Tightness enum
        public float Filled; // The Fill INSIDE the container
        public float FillCapacity;
        public float BaseWeight;
        public List<Item> Contents = new List<Item>();

        public Container(float fill, float baseWeight, List<Item> contents = null)
        {
            Fill = fill;
            FillCapacity = fill; //there may be another constructor in the future
            Weight = baseWeight;
            BaseWeight = baseWeight;
            Contents = contents ?? new List<Item>();
            CalculateCapacity();
        }

        public void CalculateCapacity()
        {
            //NOTE: A Container's Fill is constant. Weight and Filled are still calculated.
            Weight = BaseWeight;
            Filled = 0;
            foreach (var item in Contents)
            {
                Weight += item.Weight;
                Filled += item.Fill;
            }
        }
    }
}
