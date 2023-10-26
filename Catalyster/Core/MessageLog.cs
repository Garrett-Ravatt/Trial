
namespace Catalyster.Core
{
    public class MessageLog
    {
        public List<string> Messages = new List<string>();
        
        public MessageLog() { }

        public MessageLog(List<string> messages)
        {
            Messages = messages;
        }

        public void Add(string message)
        {
            Messages.Add($":: :: {message}");
        }

        // Adds a message with an identification of its source
        public void IDAdd(string source, string message)
        {
            Messages.Add($":: {source} :: {message}");
        }

        public void Clear()
        {
            Messages.Clear();
        }
    }
}
