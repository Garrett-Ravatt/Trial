using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Interfaces;
using Catalyster.Components;
using Arch.CommandBuffer;

namespace Catalyster.Models
{
    // Overwrites N of the poi, rather than a random variable number.
    // You want just 1 player, right? Not 2 or 0?
    public abstract class POIOverwriteN : IStep<World>
    {
        private readonly double _p = 1;
        private readonly int _n;

        public POIOverwriteN(int n = 1) 
        {
            _n = n;
        }

        public World Step(World world, int seed)
        {
            var buffer = new CommandBuffer(world);
            var rand = new Random(seed);
            var x = 0;
            // NOTE: This query must be run single thread.
            world.Query(in new QueryDescription().WithAll<POI>(), (Entity entity, ref Position pos) =>
            {
                if (x < _n)
                {
                    var replacement = Make(world);
                    replacement.Set<Position>(pos);
                    buffer.Destroy(entity);
                }
                x++;
            });
            buffer.Playback();
            return world;
        }

        public abstract Entity Make(World world);
    }
}
