using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using CatalysterTest.TestUtils;

namespace CatalysterTest.ComponentTesting.Interactives
{
    [TestClass]
    public class DoorTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
            var gm = GameMaster.Instance();
            gm.DungeonMap.Initialize(10, 10);
            gm.DungeonMap.Clear();
        }

        [TestMethod]
        public void DoorTest1()
        {
            var gm = GameMaster.Instance();
            var player = ExFactory.Player(gm.World);
            var door = RAWFactory.Door(gm.Stats, gm.World);
            player.Set(new Position { X = 0, Y = 0 });
            door.Set(new Position { X = 1, Y = 0 });
            gm.Update();

            // Can't walk through
            gm.Command.Move(1, 0);
            gm.Resolve();
            Assert.AreEqual(new Position { X = 0, Y = 0 }, player.Get<Position>());

            // Can walk through
            door.Get<Door>().state = DoorState.OPEN;
            gm.Command.Move(1, 0);
            gm.Resolve();
            Assert.AreEqual(new Position { X = 1, Y = 0 }, player.Get<Position>());
        }

        [TestMethod]
        public void DoorTest2()
        {
            var gm = GameMaster.Instance();
            var player = ExFactory.Player(gm.World);
            var door = RAWFactory.Door(gm.Stats, gm.World);
            player.Set(new Position { X = 0, Y = 0 });
            door.Set(new Position { X = 1, Y = 0 });
            gm.Update();

            gm.DungeonMap.UpdateFieldOfView(gm.World);
            Assert.IsFalse(gm.DungeonMap.GetCell(2, 0).IsInFov);
        }
    }
}
