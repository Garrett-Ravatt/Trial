using Catalyster.Items;

namespace Catalyster.Components
{
    // Fill (Volume) and Weight used for storing something in Inventory or a Container
    public class BasicItem : Item
    {
        public float Fill { get; set; }
        public float Weight { get; set; }
    }

    // 
    public class Fluid : Item
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
