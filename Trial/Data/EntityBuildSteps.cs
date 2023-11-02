using Arch.Core;
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
}
