using Arch.Core;
using Arch.Core.Extensions;
using Catalyster;
using Catalyster.Acts;
using Catalyster.Components;
using Catalyster.Components.Directors;
using Catalyster.Helpers;
using Catalyster.Interfaces;
using Catalyster.Messages;
using CatalysterTest.TestUtils;
using Microsoft.VisualBasic.FileIO;


namespace CatalysterTest.Messages
{
    [TestClass]
    public class IncidentalMessageTests
    {
        [TestCleanup]
        public void Initialize()
        {
            GameMaster.Instance().Reset();
        }

        [TestMethod]
        public void AttackMessageTest1()
        {
            var gm = GameMaster.Instance();
            //var gm = new GameMaster();
            GameMaster.DungeonMap.Initialize(30, 30);
            GameMaster.DungeonMap.Clear();
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

        [TestMethod]
        public void ConfirmationMessageTest()
        {
            var gm = GameMaster.Instance();
            GameMaster.DungeonMap.Initialize(30, 30);
            GameMaster.DungeonMap.SetAllWalkable();
            var world = GameMaster.World;

            var player = ExFactory.Player(world);
            var act = new DieOnPurposeAct(player.Reference());

            var hub = GameMaster.MessageLog.Hub;
            var formed = false;
            Decide? d = null;
            hub.Subscribe<ConfirmationMessage>(msg => {
                formed = true;
                Console.WriteLine(msg.Message);
                d = msg.D;
            });
            Assert.IsTrue(act.Consume().Suspended);
            Assert.IsTrue(formed);

            Assert.IsNotNull(d);
            d.Invoke(true);

            formed = false;
            Assert.IsFalse(act.Consume().Suspended);
            Assert.IsTrue(act.Resolved);
            Assert.IsFalse(formed);
        }
    }
}
