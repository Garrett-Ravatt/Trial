using Arch.Core;
using Catalyster.Core;
using Catalyster.Interfaces;
using Catalyster.Components;

namespace Catalyster.Models
{
    public class POIGen : IStep<World>
    {
        private DungeonMap Map;
        public POIGen(DungeonMap map)
        {
            Map = map;
        }

        public World Step(World world, int seed)
        {
            var rooms = Map.Rooms;

            foreach (var room in rooms)
            {
                world.Create(
                    new Position{ X = room.Center.X, Y = room.Center.Y },
                    new POI { }
                    );
            }

            return world;
        }
    }
}
