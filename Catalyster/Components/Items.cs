using Catalyster.Interfaces;

namespace Catalyster.Components
{
    // Fill (Volume) and Weight used for storing something in Inventory or a Container
    public struct Item : IItem
    {
        public float Fill { get; set; }
        public float Weight { get; set; }
    }

    // 
    public struct Fluid : IItem
    {
        public float Fill { get; set; }
        //TODO: get/set Weight act as wrappers for the density
        private float Density;
        public float Weight
        {
            get
            {
                return Fill * Density;
            }
            set
            {
                Density = value / Fill;
            }
        }

        // TODO: powder, liquid, gas (MatterState enum)
    }
}
