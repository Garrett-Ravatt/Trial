using TinyMessenger;
using Catalyster.Messages;

namespace CatalysterTest.Messages
{
    [TestClass]
    public class MessageTests
    {
        [TestMethod]
        public void MessageTest1()
        {
            var hub = new TinyMessengerHub();
            var msg = new DialogueMessage(this, "Wizard", "I, me, a Wizard!");

            var called = false;
            var token = hub.Subscribe<DialogueMessage>((msg) =>
            {
                called = (msg.Source == "Wizard" && msg.Content == "I, me, a Wizard!");
            });

            hub.Publish(msg);
            Assert.IsTrue(called);

            called = false;
            hub.Publish(msg);
            Assert.IsTrue(called);

            hub.Unsubscribe<DialogueMessage>(token);
        }
    }
}
