using Arch.Bus;

// A Muck is what I'm calling tests that don't involve code from the real application.
// Mucks are a way to see if an idea will work, then they'll be made internal and removed from unit tests.

namespace CatalysterTest.Muck
{
    public struct Message
    {
        public string Text;
    }

    public partial class MyInstanceReceiver
    {

        public MyInstanceReceiver() { Hook(); } // To start listening.

        [Event(order: 1)]
        public void OnStringEvent(ref Message @event)
        {
            Console.WriteLine(@event.Text);
        }
    }

    [TestClass]
    internal class ArchEventBusMuck
    {
        [TestMethod]
        public void Test1()
        {
            var fun = new Message { Text="funny" };
            EventBus.Send(ref fun);
        }
    }
}
