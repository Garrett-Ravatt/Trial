using Catalyster.Core;
using Catalyster.Interfaces;
using RogueSharp.Random;

// NOTE: Contains code from https://roguesharp.wordpress.com/2016/04/03/roguesharp-v3-tutorial-connecting-rooms-with-hallways/

namespace Catalyster.Models
{
    public class CorridorGen : IStep<DungeonMap>
    {
        public DungeonMap Step(DungeonMap map, int seed)
        {
            var random = new DotNetRandom(seed);
            // Iterate through each room that was generated
            // Don't do anything with the first room, so start at r = 1 instead of r = 0
            for (int r = 1; r < map.Rooms.Count; r++)
            {
                // For all remaing rooms get the center of the room and the previous room
                int previousRoomCenterX = map.Rooms[r - 1].Center.X;
                int previousRoomCenterY = map.Rooms[r - 1].Center.Y;
                int currentRoomCenterX = map.Rooms[r].Center.X;
                int currentRoomCenterY = map.Rooms[r].Center.Y;

                // Give a 50/50 chance of which 'L' shaped connecting hallway to tunnel out
                if (random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(map, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(map, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }
            return map;
        }

        // Carve a tunnel out of the map parallel to the x-axis
        private void CreateHorizontalTunnel(DungeonMap map, int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                map.SetCellProperties(x, yPosition, true, true, true);
            }
        }

        // Carve a tunnel out of the map parallel to the y-axis
        private void CreateVerticalTunnel(DungeonMap map, int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                map.SetCellProperties(xPosition, y, true, true, true);
            }
        }
    }
}
