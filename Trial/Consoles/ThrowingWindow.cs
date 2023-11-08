using SadConsole.Ansi;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.Input;
using Trial.InputStates;

namespace Trial.Consoles
{
    public class ThrowingWindow : Window
    {
        private MapConsole _mapConsole;

        // we may very well overload this constructor with other consoles that may call this popup.
        public ThrowingWindow(int width, int height, MapConsole mapConsole) : base(width, height)
        {
            _mapConsole = mapConsole;

            // Take control from the map console because I am a popup menu
            IsFocused = true;
            mapConsole.IsFocused = false;

            // This is what I am
            Title = "Select an item to throw";

            // Fit list within the margin
            var listwidth = width-2;
            var listheight = height-3;

            var listbox = new ListBox(listwidth, listheight)
            {
                Position = new Point(1, 2),
                IsFocused = true,
                UseKeyboard = true,
                UseMouse = true,
                SingleClickItemExecute = true,
            };

            listbox.SelectedItemExecuted += (sender, args) =>
            {
                CommandBobber.Throw(index: 0);
                Controls.Clear();
                Hide(); // TODO: teardown
                mapConsole.IsFocused = true;
                mapConsole.SetState(MapInputState.Throwing);
                Dispose();
            };

            foreach (string s in Program.GameMaster.Command.Inventory())
                listbox.Items.Add(s);
            Controls.Add(listbox);

            Show();
        }

        public override bool ProcessKeyboard(Keyboard keyboard)
        {
            bool handled = false;

            if (keyboard.IsKeyPressed(Keys.Escape) || keyboard.IsKeyPressed(Keys.Back))
            {
                IsFocused = false;
                _mapConsole.IsFocused = true;
            }

            if (handled)
            {
                // this is hacky (but I am at peace)
                Program.GameMaster.Update();
                Program.GameMaster.Update();
                Program.Draw();
            }

            return handled;
        }
    }
}
