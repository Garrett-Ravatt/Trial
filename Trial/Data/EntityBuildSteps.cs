using Arch.Core;
using Catalyster.Core;
using Catalyster.Models;

namespace Trial.Data
{
    public class POIPlayer : POIOverwriteN
    {
        public POIPlayer(): base(1) { }
        public override Entity Make(World world)
        {
            return EntityBuilder.Player(world);
        }
    }

    public class POIGoblin : POIOverwrite
    {
        public POIGoblin(double p = 1): base(p) { }
        public override Entity Make(World world)
        {
            return EntityBuilder.Goblin(world);
        }
    }

    public class BlackPowderWrite : WallWrite
    {
        public BlackPowderWrite(DungeonMap map): base(map) { }
        public override Entity Make(World world)
        {
            return EntityBuilder.BlackPowder(world);
        }
    }
}
