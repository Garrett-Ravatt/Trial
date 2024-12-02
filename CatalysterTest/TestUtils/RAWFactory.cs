using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Core;
using Catalyster.RAW;

namespace CatalysterTest.TestUtils
{
    public class RAWFactory
    {
        public static Entity BlackPowder(EntityStats stats, World world)
        {
            var rid = "BLACK_POWDER";
            var name = "Black Powder";
            var desc = "Traditional. Versatile.";
            if (stats.Has(rid))
                return stats.CreateIn(rid, world);

            var e = ExFactory.BlackPowder(stats.World);
            var def = new EntityDefinition(name, desc, e.Reference());
            stats.Define(def);
            return stats.CreateIn(rid, world);
        }

        public static Entity Door(EntityStats stats, World world)
        {
            var rid = "DOOR";
            var name = "Door";
            var desc = "This is a door all right";
            if (stats.Has(rid))
                return stats.CreateIn(rid, world);

            var e = ExFactory.Door(stats.World);
            var def = new EntityDefinition(name, desc, e.Reference());
            stats.Define(def);
            return stats.CreateIn(rid, world);
        }
    }
}
