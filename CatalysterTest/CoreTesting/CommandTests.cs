using Arch.Core;
using Catalyster.Core;
using Catalyster;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.Components;
using CatalysterTest.TestUtils;

namespace CatalysterTest.CoreTesting
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void CommandTest1()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(10, 10);
            var world = GameMaster.World;
            var command = gm.Command;
            
            command.Move(0, 0);

            command.Entity = ExFactory.Player(world);
            command.Move(0, 0);

            World.Destroy(world);
        }

        [TestMethod]
        public void CommandTest2()
        {
            new GameMaster();
            var world = GameMaster.World;
            var command = new Command();
            var order = new TurnOrder();

            var player = ExFactory.Player(world);
            command.Entity = order.Update(world);

            Assert.AreEqual(player, command.Entity);

            World.Destroy(world);
        }

        [TestMethod]
        public void CommandTest3()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var command = gm.Command;

            var player = ExFactory.Player(GameMaster.World);
            var iPos = player.Get<Position>();

            command.Entity = player;
            command.Move(0, 1);

            Assert.AreEqual(iPos.Y + 1, player.Get<Position>().Y);
        }

        [TestMethod]
        public void CommandTest4()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.World;

            var player = ExFactory.Player(world);

            command.Entity = player;
            command.Move(0, 1);
            command.Move(0, 1);

            Assert.IsTrue(player.Get<Energy>().Points <= 0);
            Assert.IsNull(command.Entity);

            world.Dispose();
        }

        [TestMethod]
        public void CommandTest5()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.World;

            var player = ExFactory.Player(world);
            player.Set(new Position { X=0, Y=0 });

            var items = new List<EntityReference> {
                world.Create(new Item { Fill = 2, Weight = 2 }).Reference()
            };
            player.Add(new Inventory(items));

            var enemy = ExFactory.SimpleCreature(world);
            enemy.Set(new Position { X=1, Y=0 });

            command.Entity = player;

            var target = enemy.Get<Position>();
            command.Throw(target.X, target.Y, 0);

            Assert.IsTrue(player.Get<Energy>().Points <= 100);

            world.Dispose();
        }

        [TestMethod]
        public void CommandTest6()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(40, 40);
            GameMaster.DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.World;

            var player = ExFactory.Player(world);
            player.Set(new Position { X = 0, Y = 0 });

            var items = new List<EntityReference> {
                world.Create(new Item { Fill = 2, Weight = 2 }).Reference()
            };
            player.Add(new Inventory(items));

            command.Entity = player;
            var names = command.Inventory();

            Assert.AreEqual(items.Count, names.Count);

            world.Dispose();
        }
    }
}
