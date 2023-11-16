
using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;

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
            if (Handler != null) Handler(s);
        }

        // Adds a message with an identification of its source
        public void IDAdd(string source, string message)
        {
            var s = $":: {source} :: {message}";
            Messages.Add(s);
            if (Handler != null) Handler(s);
        }

        public void IDAdd(Entity entity, string message)
        {
            if (entity.Has<Token>())
            {
                IDAdd(entity.Get<Token>().Name, message);
            }
            else
            {
                IDAdd("unknown", message);
            }
        }

        public void Clear()
        {
            Messages.Clear();
        }
    }
}
