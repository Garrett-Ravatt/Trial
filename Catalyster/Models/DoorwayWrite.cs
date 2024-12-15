using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Core;
using Catalyster.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyster.Models
{
    public abstract class DoorwayWrite : IStep<World>
    {
        private readonly DungeonMap _map;
        public DoorwayWrite(DungeonMap map) { _map = map; }

        public World Step(World world, int seed)
        {
            var rooms = _map.Rooms;

            foreach (var room in rooms)
            {
                int x = -1;
                int y = -1;

                // Search for doorways.
                // A doorway is an empty space along the border of the room
                // There may be one doorway or many
                
                
                // top
                foreach (var cell in _map.GetCellsAlongLine(room.Left, room.Top, room.Right, room.Top))
                {
                    if (cell.IsWalkable)
                    {
                        x = cell.X;
                        y = cell.Y;
                        var entity = Make(world);
                        entity.Set(new Position { X = x, Y = y });
                    }
                }

                // left
                foreach (var cell in _map.GetCellsAlongLine(room.Left, room.Top, room.Left, room.Bottom))
                {
                    if (cell.IsWalkable)
                    {
                        x = cell.X;
                        y = cell.Y;
                        var entity = Make(world);
                        entity.Set(new Position { X = x, Y = y });
                    }

                }

                // right
                foreach (var cell in _map.GetCellsAlongLine(room.Right, room.Top, room.Right, room.Bottom))
                {
                    if (cell.IsWalkable)
                    {
                        x = cell.X;
                        y = cell.Y;
                        var entity = Make(world);
                        entity.Set(new Position { X = x, Y = y });
                    }
                }

                // bottom
                foreach (var cell in _map.GetCellsAlongLine(room.Left, room.Bottom, room.Right, room.Bottom))
                {
                    if (cell.IsWalkable)
                    {
                        x = cell.X;
                        y = cell.Y;
                        var entity = Make(world);
                        entity.Set(new Position { X = x, Y = y });
                    }
                }
            }

            return world;
        }

        public abstract Entity Make(World world);
    }
}
