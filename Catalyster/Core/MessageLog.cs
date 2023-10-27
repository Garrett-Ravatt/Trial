
namespace Catalyster.Core
{
    public struct MessageMoment
    {
        public string Speaker;
        public string Message;
    }

    public delegate void Callback(string message);

    public class MessageLog
    {
        public List<string> Messages = new List<string>();
        public Callback Handler;

        public MessageLog() { }

        public MessageLog(List<string> messages)
        {
            Messages = messages;
        }

        public void Add(string message)
        {
            var s = $":: :: {message}";
            Messages.Add(s);
            Handler(s);
        }

        // Adds a message with an identification of its source
        public void IDAdd(string source, string message)
        {
            var s = $":: {source} :: {message}";
            Messages.Add(s);
            Handler(s);
        }

        public void Clear()
        {
            Messages.Clear();
        }
    }
}
