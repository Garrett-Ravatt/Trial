using Arch.Core;
using Catalyster.Core;
using Catalyster;
using Arch.Core.Extensions;
using Inventory = Catalyster.Items.Inventory;
using Catalyster.Components;
using CatalysterTest.TestUtils;
using Catalyster.Helpers;
using Catalyster.RAW;

namespace CatalysterTest.CoreTesting
{
    [TestClass]
    public class CommandTests
    {
        [TestInitialize]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void CommandTest1()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(10, 10);
            GameMaster.Instance().DungeonMap.Clear();
            var world = GameMaster.Instance().World;
            var command = gm.Command;
            
            // Move(0,0) may make the player hit themselves
            command.Move(1, 0);

            command.Entity = ExFactory.Player(world);
            command.Move(1, 0);
            gm.Update();
        }

        [TestMethod]
        public void CommandTest2()
        {
            GameMaster.Instance();
            var world = GameMaster.Instance().World;
            var command = new Command();
            var order = new TurnOrder();

            var player = ExFactory.Player(world);
            command.Entity = order.Update(world);

            Assert.AreEqual(player, command.Entity);
        }

        [TestMethod]
        public void CommandTest3()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var command = gm.Command;

            var player = ExFactory.Player(GameMaster.Instance().World);
            var iPos = player.Get<Position>();

            command.Entity = player;
            command.Move(0, 1);
            gm.Update();

            Assert.AreEqual(iPos.Y + 1, player.Get<Position>().Y);
        }

        [TestMethod]
        public void CommandTest4()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.Instance().World;

            var player = ExFactory.Player(world);
            var iPos = player.Get<Position>();

            command.Entity = player;
            player.Get<Stats>().Energy = 1;
            player.Get<Stats>().Breath = 1;
            command.Move(0, 1);
            gm.Update();

            Assert.AreEqual(iPos.Y + 1, player.Get<Position>().Y);

            Assert.IsTrue(player.Get<Stats>().Energy <= 0);
            // TODO: If there is a player, why would I want this to be null I wonder
            Assert.IsNull(command.Entity);
        }

        [TestMethod]
        public void CommandTest5()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.Instance().World;

            var player = ExFactory.Player(world);
            player.Set(new Position { X=0, Y=0 });
            GameMaster.Instance().DungeonMap.UpdateFieldOfView(world);

            var items = new List<EntityReference> {
                world.Create(new Item { Fill = 2, Weight = 2 }).Reference()
            };
            player.Add(new Inventory(items));

            var enemy = ExFactory.SimpleCreature(world);
            enemy.Set(new Position { X=1, Y=0 });

            command.Entity = player;

            var target = enemy.Get<Position>();
            //TODO: verify act resolved
            Assert.IsTrue(command.Throw(target.X, target.Y, 0));
            gm.Update();
            Assert.IsTrue(player.Get<Stats>().Energy <= 100);
        }

        [TestMethod]
        public void CommandTest6()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.Instance().World;

            var player = ExFactory.Player(world);
            player.Set(new Position { X = 0, Y = 0 });

            var items = new List<EntityReference> {
                world.Create(new Item { Fill = 2, Weight = 2 }).Reference()
            };
            player.Set(new Inventory(items));

            command.Entity = player;
            var names = command.Inventory();

            Assert.AreEqual(items.Count, names.Count);
        }

        [TestMethod]
        public void CommandTest7()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.Instance().World;

            var player = ExFactory.Player(world);
            player.Set(new Position { X = 0, Y = 0 });

            world.Create(
                new Token { Char='*', Name="Rock"},
                new Position { X=1, Y=0 },
                new Item { Fill=1f, Weight=1f }
                );

            gm.Resolve();
            command.Interact();
            gm.Resolve();

            Assert.AreEqual("Rock",
                player.Get<Inventory>().Items[0].Entity.Get<Token>().Name);
        }

        [TestMethod]
        public void CommandTest8()
        {
            var gm = GameMaster.Instance();
            GameMaster.Instance().DungeonMap.Initialize(40, 40);
            GameMaster.Instance().DungeonMap.SetAllWalkable();
            var command = gm.Command;
            var world = GameMaster.Instance().World;

            var player = ExFactory.Player(world);
            player.Set(new Position { Y = 0, X = 0 });
            GameMaster.Instance().DungeonMap.UpdateFieldOfView(world);

            var bomb = ExFactory.BasicBomb(world);
            bomb.Set(new Position { X=0, Y=0 });
            gm.Update();
            command.Interact();
            gm.Resolve();

            Assert.IsTrue(player.Get<Inventory>().Items.Count > 0);

            command.Throw(1, 1, 0);
            gm.Resolve();

            // the player should be hurt
            Assert.IsTrue(player.Get<Stats>().HP < player.Get<Stats>().Blood);
        }

        [TestMethod]
        public void CommandTest9()
        {
            var gm = GameMaster.Instance();
            gm.DungeonMap.Initialize(40, 40);
            gm.DungeonMap.SetAllWalkable();

            var p = RAWFactory.BlackPowder(gm.Stats, gm.World);
            p.Set(new Position { X = 0, Y = 0 });

            Entity? found = null;
            EntityDefinition? def = null;
            if (!SpatialHelper.ClearOrAssign(0, 0, ref found))
            {
                var rid = found.Value.Get<Token>().RID;
                def = gm.Stats.Get(rid);
            }

            Assert.AreEqual("Black Powder", def.Name);
        }
    }
}
