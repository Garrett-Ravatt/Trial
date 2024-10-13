
using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;

namespace Catalyster.Core
{
    // TODO: phase out
    public delegate void Callback(string message);

    public class MessageLog
    {
        public List<string> Messages = new List<string>();
        public Callback Handler;


        public MessageLog(List<string>? messages = null)
        {
            if (messages != null)
                Messages = messages;

            // TODO: Set up Subscriptions
        }

        public void Add(string message)
        {
            var s = $":: :: {message}";
            Messages.Add(s);
            if (Handler != null) Handler(s);
            Console.WriteLine($"MESSAGE: {s}");
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
                IDAdd("???", message);
            }
        }

        public void Clear()
        {
            Messages.Clear();
        }
    }
}
