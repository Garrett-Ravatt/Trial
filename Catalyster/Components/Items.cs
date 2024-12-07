
namespace Catalyster.Components
{
    // TODO: Form (gas, powder, fluid, object)
    public struct Item
    {
        public float Fill, Weight;
    }

    // TODO: Tightness enum
    public struct Container
    {
        public float Filled, FillCap;
    }

    // Relationship so an item can "contain" another
    // and find them without storing lists manually.
    public struct Contains { }
}
