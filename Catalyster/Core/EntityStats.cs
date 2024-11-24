using Arch.Core;

namespace Catalyster.Core
{
    public class EntityStats
    {
        public World World;
        public EntityStats()
        { 
            World = World.Create();
        }
    }
}
