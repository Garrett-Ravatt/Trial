using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Components;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Messages;
using CatalysterTest.TestUtils;


namespace CatalysterTest.Messages
{
    [TestClass]
    public class IncidentalMessageTests
    {
        [TestMethod]
        public void AttackMessageTest1()
        {
            var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(30, 30);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            var creature = ExFactory.SimpleCreature(world);
            creature.Set<IDirector>(new CrazedHunter { });
            creature.Set(new Position { X = 0, Y = 0 });

            var player = ExFactory.Player(world);
            player.Set(new Position { X = 1, Y = 1 });

            var initialDist = SpatialHelper.LazyDist(
                creature.Get<Position>(),
                player.Get<Position>());

            GameMaster.DungeonMap.UpdateFieldOfView(GameMaster.World);
            gm.Update();
            gm.Command.Wait();
            gm.Update();

            // Assert the message
            var called = false;
            GameMaster.MessageLog.Hub.Subscribe<MeleeAttackMessage>(msg => { called = true; });

            // Assert the message log
            foreach (string s in GameMaster.MessageLog.Messages)
                Console.WriteLine(s);

            Assert.IsTrue(GameMaster.MessageLog.Messages.Count >= 1);

            World.Destroy(world);
        }
    }
}
