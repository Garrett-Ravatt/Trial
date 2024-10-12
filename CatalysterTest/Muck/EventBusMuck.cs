using TinyMessenger;

namespace CatalysterTest.Muck
{
    public class MyMessage : TinyMessageBase
    {
        public string Content { get; private set; }

        public MyMessage(object sender, string content) : base(sender)
        {
            Content = content;
        }
    }

    [TestClass]
    public class EventBusMuck
    {
        [TestMethod]
        internal void BusTest1()
        {
            var messengerHub = new TinyMessengerHub();

            // Subscribe to messages
            var subscription = messengerHub.Subscribe<MyMessage>(msg =>
            {
                Assert.AreEqual(msg.Content, "Hello, TinyMessenger!");
                Console.WriteLine($"Received message: {msg.Content}");
            });

            // Publish a message
            var message = new MyMessage(this, "Hello, TinyMessenger!");
            messengerHub.Publish(message);

            // Unsubscribe
            subscription.Dispose();
        }
    }
}
