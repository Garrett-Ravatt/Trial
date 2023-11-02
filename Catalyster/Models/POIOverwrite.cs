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
        private readonly CommandBuffer _buffer;
        
        public POIOverwrite() { }
        public POIOverwrite(double p)
        { 
            _p = p;
        }

        public World Step(World world, int seed)
        {
            var rand = new Random(seed);
            world.Query(in new QueryDescription().WithAll<POI>(), (Entity entity) =>
            {
                if (rand.NextDouble() < _p)
                {
                    // TODO: Do with command buffer.
                    //entity.Remove<POI>();
                    //AddOn(entity);
                    _buffer.Remove<POI>(in entity);
                }
            });
            _buffer.Playback();
            return world;
        }

        public abstract void AddOn(CommandBuffer buffer, Entity entity);
    }
}
