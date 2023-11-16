namespace Catalyster.Items
{
    // Fill (Volume) and Weight used for storing something in Inventory or a Container
    public class BasicItem : Item
    {
    }

    // 
    public class Fluid : Item
    {
        //TODO: get/set Weight act as wrappers for the density
        //private float Density;
        //new public float Weight
        //{
        //    get
        //    {
        //        return Fill * Density;
        //    }
        //    set
        //    {
        //        Density = (float) value / Fill;
        //    }
        //}

        // TODO: powder, liquid, gas (MatterState enum)
    }
}
