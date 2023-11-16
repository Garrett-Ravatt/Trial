using Catalyster.Core;
using Catalyster.Interfaces;
using RogueSharp.Random;
using RogueSharp;

// NOTE: This contains code from https://roguesharp.wordpress.com/2016/03/26/roguesharp-v3-tutorial-simple-room-generation/

namespace Catalyster.Models
{
    public class RoomGen : IStep<DungeonMap>
    {
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;

        public RoomGen(int maxRooms, int roomMinSize, int roomMaxSize)
        {
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;
        }

        public DungeonMap Step(DungeonMap map, int seed)
        {
            var random = new DotNetRandom(seed);

            var width = map.Width;
            var height = map.Height;

            var rooms = map.Rooms;

            // Try to place as many rooms as the specified maxRooms
            // Note: Only using decrementing loop because of WordPress formatting
            for (int r = _maxRooms; r > 0; r--)
            {
                // Determine the size and position of the room randomly
                int roomWidth = random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = random.Next(0, width - roomWidth - 1);
                int roomYPosition = random.Next(0, height - roomHeight - 1);

                // All of our rooms can be represented as Rectangles
                var newRoom = new Rectangle(roomXPosition, roomYPosition,
                  roomWidth, roomHeight);

                // Check to see if the room rectangle intersects with any other rooms
                bool newRoomIntersects = rooms.Any(room => newRoom.Intersects(room));

                // As long as it doesn't intersect add it to the list of rooms
                if (!newRoomIntersects)
                {
                    rooms.Add(newRoom);
                }
            }
            // Iterate through each room that we wanted placed 
            // call CreateRoom to make it
            foreach (Rectangle room in rooms)
            {
                CreateRoom(room, map);
            }
            return map;
        }

        // Given a rectangular area on the map
        // set the cell properties for that area to true
        private void CreateRoom(Rectangle room, DungeonMap map)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    map.SetCellProperties(x, y, true, true, true);
                }
            }
        }
    }
}
