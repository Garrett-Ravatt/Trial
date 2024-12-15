using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;
using Catalyster.Core;
using Catalyster.Interfaces;
using RogueSharp;
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
                
                // top: LT and RT should not be traversed again
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

                // left: LT, RT, and LB shouldn't be traversed again
                foreach (var cell in _map.GetCellsAlongLine(room.Left, room.Top + 1, room.Left, room.Bottom))
                {
                    if (cell.IsWalkable)
                    {
                        x = cell.X;
                        y = cell.Y;
                        var entity = Make(world);
                        entity.Set(new Position { X = x, Y = y });
                    }

                }

                // right: LT, RT, LB, and RB shouldn't be traversed again
                foreach (var cell in _map.GetCellsAlongLine(room.Right + 1, room.Top, room.Right, room.Bottom))
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
                foreach (var cell in _map.GetCellsAlongLine(room.Left + 1, room.Bottom, room.Right - 1, room.Bottom))
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
