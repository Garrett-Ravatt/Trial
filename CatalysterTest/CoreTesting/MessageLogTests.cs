using Catalyster.Core;
using Catalyster.Messages;

namespace CatalysterTest.CoreTesting
{
    [TestClass]
    public class MessageLogTests
    {
        [TestMethod]
        public void MessageLogTest1()
        {
            var log = new MessageLog();
            log.Add("Heehoo! Who am I?");
            Assert.AreEqual(":: :: Heehoo! Who am I?", log.Messages.First());
        }

        [TestMethod]
        public void MessageLogTest2()
        {
            var log = new MessageLog();
            log.IDAdd("Wizard", "I, me, a Wizard!");
            Assert.AreEqual(":: Wizard :: I, me, a Wizard!", log.Messages.First());
        }

        [TestMethod]
        public void MessageLogTest3()
        {
            var log = new MessageLog();
            //hub.Subscribe<DialogueMessage>(msg => log.IDAdd(msg.Source, msg.Content));

            var msg = new DialogueMessage(this, "Wizard", "I, me, a Wizard!");
            log.Hub.Publish(msg);

            Assert.AreEqual(":: Wizard :: I, me, a Wizard!", log.Messages.First());
        }
    }
}
