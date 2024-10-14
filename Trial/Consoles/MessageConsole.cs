using SadConsole.Instructions;
using Catalyster.Messages;
using Catalyster;
using Arch.Core;
using Arch.Core.Extensions;
using Catalyster.Components;

namespace Trial.Consoles
{
    public partial class MessageConsole : Console
    {
        private DrawString _drawString;
        private Queue<string> _drawQueue = new Queue<string>();

        //private int line;
        public MessageConsole() : base(GameSettings.MessageLogWidth, GameSettings.MessageLogHeight)
        {
            // Offset to right of the map
            this.Position = new Point(GameSettings.MapWidth, 0);

            Cursor.IsEnabled = false;
            Cursor.IsVisible = true;

            Type(":: :: Your task comes to hand.");
            //GameMaster.MessageLog.Handler += Type;

            var hub = GameMaster.MessageLog.Hub;

            hub.Subscribe<DialogueMessage>(msg => IDType(msg.Source, msg.Content));

            hub.Subscribe<MeleeAttackMessage>(msg => {
                if (msg.Hit)
                    IDType(msg.Attacker, $"hits [{msg.ToHit}] for {msg.Damage} damage");
                else
                {
                    IDType(msg.Attacker, $"misses [{msg.ToHit}]");
                    IDType(msg.Defender, "successfully defends.");
                }
            });

            hub.Subscribe<RangedAttackMessage>(msg =>
            {
                if (msg.Hit)
                    IDType(msg.Attacker, $"hits [{msg.ToHit}] for {msg.Damage} damage");
                else
                {
                    IDType(msg.Attacker, $"misses [{msg.ToHit}]");
                    IDType(msg.Defender, "successfully defends.");
                }
            });

            hub.Subscribe<DeathMessage>(msg => IDType(msg.Ref.Entity.Get<Token>().Name, "dies!"));

            hub.Subscribe<WallBumpMessage>(msg => Type(":: :: You bump into the wall. You fool."));

            hub.Subscribe<ItemCollectedMessage>(msg => Type($"{msg.ItemEntity.Entity.Get<Token>().Name} Collected."));
        }

        public void IDType(EntityReference source, string message)
        {
            string name = "";
            Token t;
            if (source.IsAlive() && source.Entity.TryGet<Token>(out t))
                name = t.Name;
            IDType(name, message);
        }

        public void IDType(string source, string message)
        {
            Type($":: {source} :: {message}");
        }

        public void Type(string message)
        {
            if (_drawString==null || _drawString.IsFinished)
            {
                StartDraw(message);
            }
            else
            {
                _drawQueue.Enqueue(message);
            }
        }

        public void Type(IEnumerable<string> message)
        {
            foreach(string messageItem in message)
            {
                _drawQueue.Enqueue(messageItem);
            }

            if (_drawString == null || _drawString.IsFinished)
            {
                TryDequeue();
            }
        }

        private void StartDraw(string message)
        {
            // TODO: Type in black, and have a fake Cursor run over the text, turning it white.

            _drawString = new DrawString(ColoredString.Parser.Parse(message+"\n\r"));
            _drawString.Cursor = Cursor;
            _drawString.Position = Cursor.Position;
            _drawString.TotalTimeToPrint = 0.5f;
            _drawString.RemoveOnFinished = true;

            _drawString.Finished += TryDequeue;
            SadComponents.Add(_drawString);
        }

        private void TryDequeue(object? sender, EventArgs e)
        {
            TryDequeue();
        }

        private void TryDequeue()
        {
            if (_drawQueue.Count > 0)
            {
                var message = _drawQueue.Dequeue();
                StartDraw(message);
            }
        }
    }
}
