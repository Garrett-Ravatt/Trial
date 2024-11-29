using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.Input;
using Trial.InputStates;
using Catalyster.Core;

namespace Trial.Consoles
{
    //TODO: x or escape out of window
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
                CommandBobber.Throw(index: listbox.SelectedIndex);
                Controls.Clear();
                Hide();
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
                Program.GameMaster.Resolve();
                Program.Draw();
            }
            else
                handled = base.ProcessKeyboard(keyboard);

            return handled;
        }
    }
}
