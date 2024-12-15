using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Interfaces;
using Catalyster.Core;

namespace Catalyster.Models
{
    public abstract class WallWrite : IStep<World>
    {
        private readonly DungeonMap _map;
        public WallWrite(DungeonMap map) { _map = map; }

        public World Step(World world, int seed)
        {
            var rand = new Random(seed);
            var rooms = _map.Rooms;

            foreach (var room in rooms)
            {
                // choose a wall, then choose somewhere on it
                int x;
                int y;
                
                if (rand.Next(2) > 0) // basically a coin toss
                {
                    x = rand.Next(room.Left, room.Right);
                    if (rand.Next(2) > 0)
                        y = room.Top;
                    else
                        y = room.Bottom;
                }
                else
                {
                    y = rand.Next(room.Top, room.Bottom);
                    if (rand.Next(2) > 0)
                        x = room.Left;
                    else
                        x = room.Right;
                }

                var entity = Make(world);
                entity.Set(new Position { X = x, Y = y });
            }

            return world;
        }

        public abstract Entity Make(World world);
    }
}
