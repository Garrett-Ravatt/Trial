using Catalyster.Interfaces;

namespace Catalyster.Items
{
    public class Container : IItem
    {
        public float Fill { get; set; }
        public float Weight { get; set; }

        // TODO: Tightness enum
        public float Filled; // The Fill INSIDE the container
        public float FillCapacity;
        public float BaseWeight;
        public List<IItem> Contents = new List<IItem>();

        public Container(float fill, float baseWeight, List<IItem> contents)
        {
            Fill = fill;
            FillCapacity = fill; //there may be another constructor in the future
            Weight = baseWeight;
            BaseWeight = baseWeight;
            Contents = contents;
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
