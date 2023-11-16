using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Components;

namespace Catalyster.Models
{
    public abstract class POIOverwrite : IStep<World>
    {
        private readonly double _p;
        
        public POIOverwrite() { }
        public POIOverwrite(double p)
        { 
            _p = p;
        }

        public World Step(World world, int seed)
        {

            world.Query(in new QueryDescription().WithAll<POI>(), (Entity entity) =>
            {
                entity.Remove<POI>();
                AddOn(entity);
            });
            return world;
        }

        public abstract void AddOn(Entity entity);
    }
}
