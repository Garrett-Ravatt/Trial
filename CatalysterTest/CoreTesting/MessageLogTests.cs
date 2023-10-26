using Catalyster.Core;

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
    }
}
