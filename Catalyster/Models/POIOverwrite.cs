using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Components;
using Arch.CommandBuffer;

namespace Catalyster.Models
{
    public abstract class POIOverwrite : IStep<World>
    {
        private readonly double _p = 1;
        
        public POIOverwrite() { }
        // randomly assign p of the poi to the new entity
        public POIOverwrite(double p)
        { 
            _p = p;
        }

        public World Step(World world, int seed)
        {
            var rand = new Random(seed);
            var buffer = new CommandBuffer(world);
            world.Query(in new QueryDescription().WithAll<POI>(), (Entity entity, ref Position pos) =>
            {
                if (rand.NextDouble() < _p)
                {
                    var replacement = Make(world);
                    replacement.Set<Position>(pos);
                    buffer.Destroy(entity);
                }
            });
            buffer.Playback();
            return world;
        }

        public abstract Entity Make(World world);
    }
}
