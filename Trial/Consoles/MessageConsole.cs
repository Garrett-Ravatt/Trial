using SadConsole.Instructions;
using static System.Net.Mime.MediaTypeNames;

namespace Trial.Consoles
{
    public class MessageConsole : Console
    {
        private InstructionSet _instructionSet;
        //private int line;
        public MessageConsole() : base(GameSettings.MessageLogWidth, GameSettings.MessageLogHeight)
        {
            // Offset to right of the map
            this.Position = new Point(GameSettings.MapWidth, 0);

            // Track the instructions
            _instructionSet = new InstructionSet();

            // Printing the first message
            var startMessage = ":: :: Your task comes to hand.";
            //line = 0;
            
            Cursor.IsEnabled = false;
            Cursor.IsVisible = true;

            Type(startMessage);
            Type("heh");
            SadComponents.Add(_instructionSet);
        }

        public void Type(string message)
        {
            // TODO: Draw animation using instructions, maybe an InstructionSet, maybe a Reader, idk.

            //var typingInstruction = new DrawString(ColoredString.Parser.Parse(message));
            //typingInstruction.Cursor = Cursor;
            //typingInstruction.Position = Cursor.Position;
            //typingInstruction.TotalTimeToPrint = 3f;
            //_instructionSet.Instruct(typingInstruction);

            Cursor.Print(message).NewLine();
        }
    }
}
