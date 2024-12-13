﻿using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Acts.Interactive;
using Catalyster.Components;
using Catalyster.Components.Extensions;
using Catalyster.Interfaces;
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

            // Can open door with player action
            IAct act = new UseAct(player.Reference());
            act = act.Execute();
            Assert.AreEqual(typeof(UseDoorAct), act.GetType());
            act = act.Consume();
            Assert.IsTrue(act.Resolved);

            // Can walk through: OPEN
            gm.Command.Move(1, 0);
            gm.Resolve();
            Assert.AreEqual(new Position { X = 1, Y = 0 }, player.Get<Position>());

            // Can't close while standing here: OPEN
            act = new UseAct(player.Reference());
            act = act.Consume();
            Assert.AreEqual(door.Get<Door>().state, DoorState.OPEN);
            gm.Command.Move(-1, 0);
            gm.Resolve();

            // Can close: CLOSE
            act = new UseAct(player.Reference());
            act = act.Consume();
            Assert.AreEqual(door.Get<Door>().state, DoorState.CLOSED);
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

        [TestMethod]
        public void DoorTest3()
        {
            var gm = GameMaster.Instance();
            var e = RAWFactory.Door(gm.Stats, gm.World);
            var p = e.Get<Position>();
            ref var t = ref e.Get<Token>();

            // Clear Space
            Assert.IsTrue(gm.DungeonMap.GetCell(p.X, p.Y).IsWalkable);

            var door = e.Get<Door>();
            door.state.UpdateMap(p);

            // Blocked space omg
            Assert.IsFalse(gm.DungeonMap.GetCell(p.X, p.Y).IsWalkable);

            door.state.SetUpdate(DoorState.OPEN, p, ref t);
            Assert.AreEqual(DoorState.OPEN, door.state);
            Assert.IsTrue(gm.DungeonMap.GetCell(p.X, p.Y).IsWalkable);
            Assert.AreEqual('-', e.Get<Token>().Char);
        }
    }
}
