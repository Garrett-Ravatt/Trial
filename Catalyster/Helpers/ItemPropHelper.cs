using Arch.Core;
using Arch.Core.Extensions;
using Arch.Relationships;
using Catalyster.Components;

namespace Catalyster.Helpers
{
    public static class ItemPropHelper
    {
        // try to put something in a container.
        // returns true if it could put the content in the container
        public static bool Contain(Entity container, Entity content)
        {
            if (container.Has<Container, Item>() && content.Has<Item>())
            {
                ref var item = ref content.Get<Item>();

                //TODO: Care about capacity
                ref var c = ref container.Get<Container>();
                ref var i = ref container.Get<Item>();

                // update internal Filled
                c.Filled += item.Fill;

                // update weight
                i.Weight += item.Weight;

                container.AddRelationship<Contains>(content);
                return true;
            }
            return false;
        }

        // Returns true if content is removed from container
        public static bool Detain(Entity container, Entity content)
        {
            if (container.Has<Container, Item>() && content.Has<Item>()
                && container.HasRelationship<Contains>(content))
            {
                ref var item = ref content.Get<Item>();

                ref var c = ref container.Get<Container>();
                ref var i = ref container.Get<Item>();

                // update internal Filled
                c.Filled -= item.Fill;

                // update weight
                i.Weight -= item.Weight;

                container.RemoveRelationship<Contains>(content);
                return true;
            }
            return false;
        }

        // moving one item from its first container to another
        public static bool ReContain(Entity source, Entity dest, Entity content)
        {
            if (Detain(source, content))
                if (Contain(dest, content))
                    return true;
            return false;
        }
    }
}
