using SadConsole.Instructions;
using static System.Net.Mime.MediaTypeNames;

namespace Trial.Consoles
{
    public class MessageConsole : Console
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

            //Cursor.Print(message).NewLine();
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
            _drawString.TotalTimeToPrint = 0.7f;
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
