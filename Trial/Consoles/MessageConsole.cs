
using SadConsole.Instructions;
using static System.Net.Mime.MediaTypeNames;

namespace Trial.Consoles
{
    public class MessageConsole : Console
    {
        private InstructionSet InstructionSet;
        public MessageConsole() : base(GameSettings.MessageLogWidth, GameSettings.MessageLogHeight)
        {
            // Offset to right of the map
            this.Position = new Point(GameSettings.MapWidth, 0);

            var startMessage = ":: :: Your task comes to hand.";

            Cursor.Position = new Point(0, 0);
            Cursor.IsEnabled = false;
            Cursor.IsVisible = true;

            Type(startMessage);
        }

        public void Type(string message)
        {
            // TODO: The animation using instructions, maybe an InstructionSet, maybe a Reader, idk.
            //var typingInstruction = new DrawString(ColoredString.Parser.Parse(message));
            //typingInstruction.TotalTimeToPrint = 0.5f;
            //typingInstruction.Cursor = Cursor;
            //SadComponents.Add(typingInstruction);
            Cursor.Print(message).NewLine();
        }
    }
}
