using Arch.Core;
using RogueSharp;
using Catalyster.Core;
using Catalyster.Components;

namespace CatalysterTest
{

    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void MakeMap()
        {
            var map = new Map();
            map.Initialize(10, 10);

            var room = new Rectangle(
                3, // x coord
                3, // y coord
                4, // x width
                4 // y height
                );

            for (int x = room.Left + 1; x<= room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    map.SetCellProperties(x, y, true, true, true);
                }
            }

            Assert.IsTrue(map.GetCell(5, 5).IsWalkable);
            Assert.IsFalse(map.GetCell(5, 5).IsInFov);
        }

        [TestMethod]
        public void PlayerSight()
        {
            // Make map
            var map = new DungeonMap();
            map.Initialize(10, 10);
            var room = new Rectangle(3, 3, 4, 4);
            for (int x = room.Left + 1; x <= room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    map.SetCellProperties(x, y, true, true, true);
                }
            }

            // Make world with Player
            var world = World.Create();
            world.Create(
                new Player { },
                new Position { X = 5, Y = 5 },
                new Token { Name = "Alchymer", Char = '@', Color = 0xffffff },
                new Sense { Range = 2 }
            );

            // Calculate sight
            map.UpdateFieldOfView(world);

            Assert.IsTrue(map.GetCell(5, 6).IsInFov);
            Assert.IsFalse(map.GetCell(8, 8).IsInFov);
        }
    }
}
