
using Arch.Core;
using Arch.Core.Extensions;
using TinyMessenger;
using Catalyster.Components;
using Catalyster.Messages;

namespace Catalyster.Core
{
    // TODO: phase out
    public delegate void Callback(string message);

    public class MessageLog
    {
        public List<string> Messages = new List<string>();
        public Callback Handler;

        public TinyMessengerHub Hub = new TinyMessengerHub();

        public MessageLog(List<string>? messages = null)
        {
            if (messages != null)
                Messages = messages;

            // Set up Message Subscriptions
            Hub.Subscribe<DialogueMessage>(msg => IDAdd(msg.Source, msg.Content));

            Hub.Subscribe<MeleeAttackMessage>(msg => {
                if (msg.Hit)
                    IDAdd(msg.Attacker, $"hits [{msg.ToHit}] for {msg.Damage} damage");
                else
                {
                    IDAdd(msg.Attacker, $"misses [{msg.ToHit}]");
                    IDAdd(msg.Defender, "successfully defends.");
                }
            });

            Hub.Subscribe<DeathMessage>(msg => IDAdd(msg.Ref.Entity.Get<Token>().Name, "dies!"));
        }

        public void Add(string content)
        {
            var s = $":: :: {content}";
            Messages.Add(s);
            if (Handler != null) Handler(s);
            Console.WriteLine($"MESSAGE: {s}");
        }

        // Adds a content with an identification of its source
        public void IDAdd(string source, string content)
        {
            var s = $":: {source} :: {content}";
            Messages.Add(s);
            if (Handler != null) Handler(s);
        }

        public void IDAdd(Entity entity, string content)
        {
            if (entity.Has<Token>())
            {
                IDAdd(entity.Get<Token>().Name, content);
            }
            else
            {
                IDAdd("???", content);
            }
        }

        public void Clear()
        {
            Messages.Clear();
        }
    }
}
